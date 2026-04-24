using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record RemoveOrderPositionCommand(long OrderId, long ProductId) : ICommand;