using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using XChat.Api.Exceptions.Http;
using RouteHandler = System.Func<string, XChat.Api.Helpers.Http.Response>;
using HttpHandlers = System.Collections.Generic.Dictionary
    <System.Net.Http.HttpMethod, System.Collections.Generic.Dictionary
        <string, System.Func<string, XChat.Api.Helpers.Http.Response>>>;

namespace XChat.Api.Services.Http;

internal class HttpService
{
    private readonly HttpHandlers _handlers = [];
    private readonly HttpListener _server;
    private readonly ILogger<HttpService> _logger;
    private readonly int _port;

    public HttpService(IConfiguration configuration, ILogger<HttpService> logger)
    {
        _port = configuration.GetValue<int>("EndPoint:Port");
        _logger = logger;
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

    public async Task StartAsync()
    {
        _server.Start();
        _logger.LogInformation("HTTP Server listening on port {Port}", _port);

        while (true)
        {
            try
            {
                var context = await _server.GetContextAsync();
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

        var method = HttpMethod.Parse(request.HttpMethod);
        var path = request.Url?.AbsolutePath ?? string.Empty;

        _logger.LogInformation("Incoming request: {Method} {Path}", method, path);

        if (_handlers.ContainsKey(method) && _handlers[method].ContainsKey(path))
        {
            try
            {
                string requestBody = string.Empty;
                if (request.HasEntityBody && request.InputStream != null)
                {
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    requestBody = await reader.ReadToEndAsync();
                }

                var handler = _handlers[method][path];
                var result = handler.Invoke(requestBody);

                response.StatusCode = (int)result.StatusCode;

                foreach (var header in result.Headers)
                {
                    response.AddHeader(header.Key, header.Value);
                }

                byte[] buffer = result.GetBodyBytes();
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

                _logger.LogInformation("Request handled successfully: {Method} {Path}", method, path);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                var errorText = $"Error: {ex.Message}";
                var buffer = System.Text.Encoding.UTF8.GetBytes(errorText);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

                _logger.LogError(ex, "Error handling request {Method} {Path}", method, path);
            }
        }
        else
        {
            response.StatusCode = 404;
            var buffer = System.Text.Encoding.UTF8.GetBytes("Not Found");
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, CancellationToken.None);

            _logger.LogWarning("No handler found for {Method} {Path}", method, path);
        }

        response.Close();
    }
}
