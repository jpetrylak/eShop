using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetPagedOrderHistoryQueryHandler : IQueryHandler<GetPagedOrderHistoryQuery, PagedResult<OrderHistoryModel>>
{
    private readonly EShopDbContext _context;

    public GetPagedOrderHistoryQueryHandler(EShopDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<OrderHistoryModel>> HandleAsync(GetPagedOrderHistoryQuery query, CancellationToken ct)
    {
        PagedResult<OrderHistoryModel> pagedResult = await _context.OrderHistoryLogs
            .AsNoTracking()
            .Where(l => l.OrderId == query.OrderId)
            .Select(SelectExpression())
            .PaginateAsync(query.OrderBy, query.SortOrder, query.Page, query.Results);

        return pagedResult;
    }

    private Expression<Func<OrderHistoryLog, OrderHistoryModel>> SelectExpression() =>
        l => new OrderHistoryModel
        {
            Message = l.Message,
            Occured = l.Occured
        };
}