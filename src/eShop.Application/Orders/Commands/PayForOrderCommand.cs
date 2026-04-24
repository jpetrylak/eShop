using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record PayForOrderCommand(long OrderId) : ICommand;