using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderShippedNotifierCommand(Guid OrderGuid, DateTime ShippingDateTime)
    : ICommand;