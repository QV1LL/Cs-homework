using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.RegularExpressions;
using XChat.Api.Exceptions.Http;
using XChat.Api.Helpers.Http;
using HttpHandlers = System.Collections.Generic.Dictionary
    <System.Net.Http.HttpMethod, System.Collections.Generic.Dictionary
        <string, System.Func<XChat.Api.Helpers.Http.Request, System.Threading.Tasks.Task<XChat.Api.Helpers.Http.Response>>>>;
using RouteHandler = System.Func<XChat.Api.Helpers.Http.Request, System.Threading.Tasks.Task<XChat.Api.Helpers.Http.Response>>;

namespace XChat.Api.Services.Http;

internal class HttpService
{
    private readonly HttpHandlers _handlers = [];
    private readonly HttpListener _server;
    private readonly ILogger<HttpService> _logger;
    private readonly IConfiguration _configuration;
    private readonly int _port;
    private readonly WebSocketService _webSocketService;

    public HttpService(IConfiguration configuration, ILogger<HttpService> logger, WebSocketService webSocketService)
    {
        _configuration = configuration;
        _port = configuration.GetValue<int>("EndPoint:Port");
        _logger = logger;
        _webSocketService = webSocketService;
        _server = new HttpListener();
        _server.Prefixes.Add($"http://+:{_port}/");

        _logger.LogInformation("HttpService created. Listening on port {Port}", _port);
    }

    public void AddHandler(HttpMethod method, string route, RouteHandler handler)
    {
        if (!_handlers.ContainsKey(method))
            _handlers[method] = [];

        if (_handlers[method].ContainsKey(route))
            throw new CannotAddTwoHandlersOnEndPointException();

        _handlers[method][route] = handler;
        _logger.LogInformation("Handler added for {Method} {Route}", method, route);
    }

    public string[] GetHandledRoutes()
    {
        return _handlers.SelectMany(methodPair => methodPair.Value.Keys).ToArray();
    }

    public async Task StartAsync()
    {
        _server.Start();
        _logger.LogInformation("HTTP Server listening on port {Port}", _port);

        while (true)
        {
            try
            {
                var context = await _server.GetContextAsync();

                _logger.LogInformation($"Http request from {context.Request.RemoteEndPoint}");

                _ = HandleRequestAsync(context);
            }
            catch (HttpListenerException ex)
            {
                _logger.LogError(ex, "HttpListener exception occurred.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception in listener loop.");
            }
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.IsWebSocketRequest)
        {
            await HandleWebSocketRequestAsync(context);
            return;
        }

        var method = HttpMethod.Parse(request.HttpMethod);
        var route = request.Url?.PathAndQuery ?? string.Empty;

        _logger.LogInformation("Incoming request: {Method} {Path}", method, route);

        if (TryGetHandler(method, route, out RouteHandler? handler))
        {
            try
            {
                string requestBody = string.Empty;
                if (request.HasEntityBody && request.InputStream != null)
                {
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    requestBody = await reader.ReadToEndAsync();
                }

                var requestDto = new Request(request.RawUrl ?? string.Empty, requestBody);
                var result = await handler!.Invoke(requestDto);
                response.StatusCode = (int)result.StatusCode;
                response.Headers.Add("Access-Control-Allow-Origin", _configuration["AllowedHosts"] ?? "https://localhost:7034");

                foreach (var header in result.Headers)
                    response.AddHeader(header.Key, header.Value);

                byte[] buffer = result.GetBodyBytes();
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

                _logger.LogInformation("Request handled successfully: {Method} {Path}", method, route);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                var errorText = $"Error: {ex.Message}";
                var buffer = System.Text.Encoding.UTF8.GetBytes(errorText);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

                _logger.LogError(ex, "Error handling request {Method} {Path}", method, route);
            }
        }
        else
        {
            response.StatusCode = 404;
            var buffer = System.Text.Encoding.UTF8.GetBytes("Not Found");
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

            _logger.LogWarning("No handler found for {Method} {Path}", method, route);
        }

        response.Close();
    }

    private async Task HandleWebSocketRequestAsync(HttpListenerContext context)
    {
        try
        {
            var webSocketContext = await context.AcceptWebSocketAsync(null);
            _webSocketService.AddClientAsync(webSocketContext.WebSocket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket handshake failed");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.Close();
        }
    }

    private bool TryGetHandler(HttpMethod method, string path, out RouteHandler? handler)
    {
        handler = null;

        if (!_handlers.TryGetValue(method, out var routes))
            return false;

        foreach (var routesHandlers in routes)
        {
            var routePattern = routesHandlers.Key;

            string[] parts = routePattern.Split('?', 2);
            string pathPart = parts[0];
            string queryPart = parts.Length > 1 ? parts[1] : null!;

            string pathPattern = "^" + Regex.Replace(pathPart, @"\{[^}]+\}", @"[^/]+");

            string fullPattern;
            if (string.IsNullOrEmpty(queryPart))
            {
                fullPattern = pathPattern + @"(?:\?.*)?";
            }
            else
            {
                string[] querySegments = queryPart.Split('&', StringSplitOptions.RemoveEmptyEntries);
                var queryPatterns = new List<string>();
                foreach (var seg in querySegments)
                {
                    string qpat = Regex.Replace(seg, @"\{[^}]+\}", @"[^&]+");
                    queryPatterns.Add(qpat);
                }

                string queryPattern = string.Join("&", queryPatterns) + @"(?:&.*)?";
                fullPattern = pathPattern + @"\?" + queryPattern;
            }

            if (Regex.IsMatch(path, fullPattern + "$", RegexOptions.IgnoreCase))
            {
                handler = routesHandlers.Value;
                return true;
            }
        }

        return false;
    }
}