using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record ShipOrderCommand(long OrderId) : ICommand;