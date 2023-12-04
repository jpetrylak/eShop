using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderCreatedNotifierCommand(Guid OrderGuid, string UserEmail, string ShippingAddress)
    : ICommand;