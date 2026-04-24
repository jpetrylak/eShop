using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;

namespace eShop.Application.Orders.Queries;

public record GetPagedOrderHistoryQuery(long OrderId, string OrderBy, int Page, int Results, string SortOrder)
    : IQuery<PagedResult<OrderHistoryModel>>, IPagedQuery;