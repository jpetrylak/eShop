using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;

namespace eShop.Application.Orders.Queries;

public record GetOrderHistoryQuery(long OrderId) : IQuery<IEnumerable<OrderHistoryModel>>;