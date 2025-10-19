namespace CurrencyExchange.Api.Services;

internal static class LogService
{
    private static readonly object _lock = new();
    private static Thread? _statusThread;
    private static bool _statusRunning;
    private static DateTime _statusStartTime;
    private static int _statusLine;

    public static void StartLogStatus()
    {
        lock (_lock)
        {
            _statusStartTime = DateTime.Now;
            _statusRunning = true;
            _statusLine = Console.CursorTop;

            _statusThread = new Thread(() =>
            {
                while (_statusRunning)
                {
                    lock (_lock)
                    {
                        LogTime();
                    }
                    Thread.Sleep(1000);
                }
            })
            {
                IsBackground = true
            };
            _statusThread.Start();
        }
    }

    public static void EndLogStatus()
    {
        _statusRunning = false;
        _statusThread?.Join();
        lock (_lock)
        {
            Console.SetCursorPosition(0, _statusLine);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, _statusLine);
        }
    }

    public static void Info(string message)
    {
        PrintLog("INFO", message, ConsoleColor.Cyan);
    }

    public static void Error(string message)
    {
        PrintLog("ERROR", message, ConsoleColor.Red);
    }

    public static void ClientMessage(string client, string message)
    {
        PrintLog($"CLIENT {client}", message, ConsoleColor.Yellow);
    }

    private static void PrintLog(string level, string message, ConsoleColor color)
    {
        lock (_lock)
        {
            Console.SetCursorPosition(0, _statusLine);
            Console.ForegroundColor = color;
            string logText = $"[{DateTime.Now:T}] [{level}] {message}";
            Console.WriteLine(logText.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
            _statusLine = Console.CursorTop;
            
            if (_statusRunning) LogTime();
        }
    }

    private static void LogTime()
    {
        Console.SetCursorPosition(0, _statusLine);
        TimeSpan uptime = DateTimeOffset.Now - _statusStartTime;
        string statusText = $"Server running: {uptime:hh\\:mm\\:ss}  Press 'S' to stop";
        Console.Write(statusText.PadRight(Console.WindowWidth - 1));
        Console.SetCursorPosition(0, _statusLine);
    }
}