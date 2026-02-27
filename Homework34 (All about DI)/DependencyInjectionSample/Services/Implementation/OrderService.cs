using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class OrderService(IEmailSender emailSender, IInventoryCheck inventoryCheck);