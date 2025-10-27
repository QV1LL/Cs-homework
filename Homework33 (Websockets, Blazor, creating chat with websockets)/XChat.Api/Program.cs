using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XChat.Api.Controllers;
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

var httpService = host.Services.GetRequiredService<HttpService>();

var messageController = host.Services.GetRequiredService<MessageController>();
var authController = host.Services.GetRequiredService<AuthController>();

httpService.AddHandler(HttpMethod.Post, "/api/messages", messageController.CreateMessage);

_ = httpService.StartAsync();

await host.RunAsync();
