using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;

namespace eShop.Application.Orders.Queries;

public record GetPagedOrderPositionsQuery(long OrderId, string OrderBy, int Page, int Results, string SortOrder)
    : IQuery<PagedResult<OrderPositionModel>>, IPagedQuery;