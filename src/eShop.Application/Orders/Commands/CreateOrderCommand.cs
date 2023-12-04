using Convey.CQRS.Commands;
using eShop.Application.Orders.Models;

namespace eShop.Application.Orders.Commands;

public record CreateOrderCommand(Guid OrderGuid, string UserEmail, string ShippingAddress,
    IEnumerable<CreateOrderPositionModel> Positions)
    : ICommand;