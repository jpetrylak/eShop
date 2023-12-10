using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderShippedNotifierCommand(long OrderId, DateTime ShippingDateTime) : ICommand;
