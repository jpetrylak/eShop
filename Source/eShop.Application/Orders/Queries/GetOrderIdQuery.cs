using Convey.CQRS.Queries;

namespace eShop.Application.Orders.Queries;

public record GetOrderIdQuery(Guid OrderGuid) : IQuery<long>;