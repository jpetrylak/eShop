using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record CreateOrderPositionCommand(long OrderId, long ProductId, int Amount) : ICommand;