using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderCreatedNotifierCommand(long OrderId, string UserEmail, string ShippingAddress)
    : ICommand;