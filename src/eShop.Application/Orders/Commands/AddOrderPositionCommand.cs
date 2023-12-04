using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record AddOrderPositionCommand(long OrderId, long ProductId, int Amount) : ICommand;