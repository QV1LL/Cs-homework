using CurrencyExchange.Api.Helpers;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CurrencyExchange.Api.Services;

internal class ServerHostService : IDisposable
{
    private readonly CurrencyService _currencyService;
    private readonly Configuration _configuration;
    private IPEndPoint _endPoint;

    private int _maxClientsCount;
    private int _maxUserMessageCount;
    private int _banTime;
    private int _connectedClientsCount = 0;
    private bool _isRunning = true;
    private readonly CancellationTokenSource _cts;

    public ServerHostService(CurrencyService currencyService, Configuration configuration)
    {
        _currencyService = currencyService;
        _configuration = configuration;
        _cts = new CancellationTokenSource();

        SetPort();
        SetMaxClientsCount();
        SetMaxUserMessageCount();
        SetBanTime();
    }

    public async Task Run()
    {
        if (_endPoint == null)
        {
            LogService.Error("Error: end point doesn't set up");
            return;
        }

        var tasks = new List<Task>();
        using var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            server.Bind(_endPoint!);
            server.Listen(5);
            LogService.Info($"Server started on port {_endPoint.Port} (max clients: {_maxClientsCount}, max messages per user: {_maxUserMessageCount}, ban time: {_banTime} min)");
        }
        catch (SocketException ex)
        {
            LogService.Error($"Bind/Listen failed on {_endPoint}: {ex.SocketErrorCode} - {ex.Message}");
            return;
        }

        _ = Task.Run(HandleServerStop);

        while (_isRunning)
        {
            try
            {
                var client = await server.AcceptAsync(_cts.Token);
                Interlocked.Increment(ref _connectedClientsCount);
                var clientTask = HandleClient(client);
                _ = clientTask.ContinueWith(_ => Interlocked.Decrement(ref _connectedClientsCount));
                tasks.Add(clientTask);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                if (_isRunning)
                {
                    LogService.Error($"Accept error: {ex.Message}");
                }
                else
                {
                    LogService.Info("Shutting down server...");
                }
            }
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            LogService.Error($"Shutdown wait error: {ex.Message}");
        }
    }

    private void HandleServerStop()
    {
        LogService.StartLogStatus();
        LogService.Info("Press 'S' key to stop server");

        while (_isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.S)
                {
                    _isRunning = false;
                    _cts.Cancel();
                }
            }

            Thread.Sleep(100);
        }

        LogService.EndLogStatus();
    }

    private void SetPort()
    {
        var portResult = _configuration.Get<int>("Port");

        if (portResult.IsFailed)
        {
            LogService.Error($"Failed to load port from config: {string.Join(", ", portResult.Errors.Select(e => e.Message))}");
            return;
        }

        var port = portResult.Value;

        if (!IsPortFree(port))
        {
            LogService.Error($"Port {port} is already in use.");
            return;
        }

        _endPoint = new IPEndPoint(IPAddress.Any, port);
        LogService.Info($"Endpoint set: {_endPoint}");
    }

    private void SetMaxClientsCount()
    {
        var maxClientsCountResult = _configuration.Get<int>("MaxConnectionsCount");

        if (maxClientsCountResult.IsFailed)
        {
            LogService.Error($"Failed to load max connections from config: {string.Join(", ", maxClientsCountResult.Errors.Select(e => e.Message))}");
            return;
        }

        _maxClientsCount = maxClientsCountResult.Value;
    }

    private void SetMaxUserMessageCount()
    {
        var maxMsgResult = _configuration.Get<int>("MaxUserMessageCount");

        if (maxMsgResult.IsFailed)
        {
            LogService.Error($"Failed to load max user message count from config: {string.Join(", ", maxMsgResult.Errors.Select(e => e.Message))}");
            return;
        }

        _maxUserMessageCount = maxMsgResult.Value;
    }

    private void SetBanTime()
    {
        var banTimeResult = _configuration.Get<int>("BanTime");

        if (banTimeResult.IsFailed)
        {
            LogService.Error($"Failed to load ban time from config: {string.Join(", ", banTimeResult.Errors.Select(e => e.Message))}");
            return;
        }

        _banTime = banTimeResult.Value;
    }

    private static bool IsPortFree(int port)
    {
        var ipProps = IPGlobalProperties.GetIPGlobalProperties();
        var tcpInUse = ipProps.GetActiveTcpListeners().Any(p => p.Port == port);
        var udpInUse = ipProps.GetActiveUdpListeners().Any(p => p.Port == port);
        return !(tcpInUse || udpInUse);
    }

    private async Task HandleClient(Socket client)
    {
        var userResult = await UserService.LoginAsync(client);
        var remote = client.RemoteEndPoint?.ToString() ?? "Unknown";

        if (userResult.IsFailed)
        {
            LogService.Info($"Authentication failed for host {remote}");
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Interlocked.Decrement(ref _connectedClientsCount);
            return;
        }

        var user = userResult.Value;
        if (user.IsBanned())
        {
            LogService.Info($"Banned user {user.Username} from {remote} attempted connection");
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            Interlocked.Decrement(ref _connectedClientsCount);
            return;
        }

        remote = $"{remote}@{user.Username}";
        int userMessageCount = 0;

        if (_connectedClientsCount > _maxClientsCount)
        {
            LogService.Info($"Rejecting {remote}: Max count of clients ({_maxClientsCount}) reached. Sending rejection message.");

            try
            {
                using var stream = new NetworkStream(client, ownsSocket: false);
                using var writer = new StreamWriter(stream) { AutoFlush = true };

                string rejectionMsg = "Server full: Maximum connections reached. Please try again later.";
                await writer.WriteLineAsync(rejectionMsg);
                await writer.FlushAsync();

                LogService.ClientMessage(remote, $"Sent rejection: {rejectionMsg}");
            }
            catch (Exception ex)
            {
                LogService.Error($"Failed to send rejection message to {remote}: {ex.Message}");
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

            Interlocked.Decrement(ref _connectedClientsCount);
            return;
        }

        LogService.Info($"Client connected: {remote}");

        try
        {
            using var stream = new NetworkStream(client);
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            await writer.WriteLineAsync("Connected to Currency Exchange Server");
            await writer.WriteLineAsync("Format: <FROM> <TO>");
            await writer.WriteLineAsync("Example: USD EUR");

            string? line;
            while ((line = await reader.ReadLineAsync(_cts.Token)) != null)
            {
                LogService.ClientMessage(remote, $"Received: {line}");

                if (string.Equals(line, "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    await writer.WriteLineAsync("Invalid format. Example: USD EUR");
                    continue;
                }

                string from = parts[0].ToUpperInvariant();
                string to = parts[1].ToUpperInvariant();

                var rateResult = _currencyService.GetExchangeRate(from, to);
                if (rateResult.IsFailed)
                {
                    await writer.WriteLineAsync($"Error: {rateResult.Errors[0].Message}");
                    continue;
                }

                double rate = rateResult.Value;
                string resultMsg = $"1 {from} = {rate:F4} {to}";

                await writer.WriteLineAsync(resultMsg);
                LogService.ClientMessage(remote, $"Response: {resultMsg}");

                userMessageCount++;
                if (userMessageCount > _maxUserMessageCount)
                {
                    var banResult = UserService.BanUser(user.Username, _banTime);
                    if (banResult.IsSuccess)
                    {
                        LogService.Info($"Banned {user.Username} for {_banTime} minutes due to excessive messages ({userMessageCount})");
                    }
                    else
                    {
                        LogService.Error($"Failed to ban {user.Username}: {string.Join(", ", banResult.Errors.Select(e => e.Message))}");
                    }

                    await writer.WriteLineAsync($"You have been banned for {_banTime} minutes due to excessive requests.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            LogService.Error($"Client {remote} error: {ex.Message}");
        }
        finally
        {
            LogService.Info($"Client disconnected: {remote}");
            client.Close();
        }

        Interlocked.Decrement(ref _connectedClientsCount);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cts.Dispose();
        }
    }

    ~ServerHostService()
    {
        Dispose(false);
    }
}