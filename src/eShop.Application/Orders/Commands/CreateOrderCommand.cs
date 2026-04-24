using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record CreateOrderCommand(Guid OrderGuid, string UserEmail, string ShippingAddress) : ICommand;
