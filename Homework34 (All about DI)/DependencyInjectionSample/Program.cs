using DependencyInjectionSample.Services.Contracts;
using DependencyInjectionSample.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<ITrafficCounter, TrafficCounter>();
services.AddTransient<IEmailSender, SmtpEmailSender>();
services.AddTransient<IInventoryCheck, LocalInventory>();
services.AddTransient<OrderService>();
services.AddTransient<INotificationHandler, SmsHandler>();
services.AddTransient<INotificationHandler, PushNotificationHandler>();
services.AddTransient<IThermostat, ThermostatService>();
services.AddTransient<ILightController, LightController>();
services.AddTransient<ISecuritySystem, SecurityService>();
services.AddTransient<SmartHomeManager>();
services.AddTransient<IPaymentProcessor, MastercardProcessor>();
services.AddTransient<CheckoutService>();
services.AddSingleton<ILogger>(provider => new FileLogger("logs.txt"));

var provider = services.BuildServiceProvider();

// Level 1 Task 1
var trafficService = provider.GetRequiredService<ITrafficCounter>();
trafficService.IncreaseCount(1);
Console.WriteLine(trafficService.Count);

var anotherTrafficService = provider.GetRequiredService<ITrafficCounter>();
anotherTrafficService.IncreaseCount(1);
Console.WriteLine(anotherTrafficService.Count);
Console.WriteLine(trafficService == anotherTrafficService);
// The count is 2 and references are equal

// Level 1 Task 2
var orderService = provider.GetService<OrderService>();

// Level 1 Task 3

const string message = "I need an offer!";
var notificationHandlers = provider.GetServices<INotificationHandler>();
foreach (var notificationHandler in notificationHandlers) notificationHandler.Send(message);

// Level 2 Task 1

var smartHomeManager = provider.GetRequiredService<SmartHomeManager>();
smartHomeManager.ActivateEveningMode();

// Level 2 Task 2

var checkoutService = provider.GetRequiredService<CheckoutService>();
checkoutService.Checkout();

// Level 2 Task 3

var logger = provider.GetRequiredService<ILogger>();
logger.Log("Hello, logger!");
logger.Log("Another message");