using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using XChat.Api.Controllers;
using XChat.Api.Helpers.Http;
using XChat.Api.Persistence;
using XChat.Api.Services.Http;
using XChat.Api.Services.Message;
using XChat.Api.Services.User;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<XChatContext>(optionsBuilder =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        });

        services.AddSingleton<HttpService>();
        services.AddSingleton<WebSocketService>();

        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IUserService, UserService>();

        services.AddScoped<MessageController>();
        services.AddScoped<AuthController>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();
var httpService = host.Services.GetRequiredService<HttpService>();

var messageController = host.Services.GetRequiredService<MessageController>();
var authController = host.Services.GetRequiredService<AuthController>();

httpService.AddHandler(HttpMethod.Post, "/api/messages", messageController.CreateMessageAsync);
httpService.AddHandler(HttpMethod.Get, "/api/messages/history?before={time}&count={size}", messageController.GetOlderMessagesAsync);
httpService.AddHandler(HttpMethod.Get, "/api/messages/recent?count={size}", messageController.GetRecentMessagesAsync);

httpService.AddHandler(HttpMethod.Post, "/api/auth/register", authController.RegisterAsync);
httpService.AddHandler(HttpMethod.Post, "/api/auth/login", authController.LoginAsync);

foreach(var route in httpService.GetHandledRoutes())
    httpService.AddHandler(HttpMethod.Options, route, async _ =>
    {
        var response = new Response<string>(HttpStatusCode.OK);
        response.Headers.Add("Access-Control-Allow-Origin", configuration["AllowedHosts"] ?? "https://localhost:7034");
        response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        return response;
    });

_ = httpService.StartAsync();

await host.RunAsync();
