using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;

namespace eShop.Application.Orders.Queries;

public record GetOrderQuery(long OrderId) : IQuery<OrderModel>;