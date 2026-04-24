using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class
    GetPagedOrderPositionsQueryHandler : IQueryHandler<GetPagedOrderPositionsQuery, PagedResult<OrderPositionModel>>
{
    private readonly EShopDbContext _dbContext;

    public GetPagedOrderPositionsQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<OrderPositionModel>> HandleAsync(GetPagedOrderPositionsQuery query,
        CancellationToken ct)
    {
        PagedResult<OrderPositionModel> pagedResult = await _dbContext.OrderPositions
            .AsNoTracking()
            .Where(o => o.Id == query.OrderId)
            .Select(SelectExpression())
            .PaginateAsync(query.OrderBy, query.SortOrder, query.Page, query.Results);

        return pagedResult;
    }

    private Expression<Func<OrderPosition, OrderPositionModel>> SelectExpression() =>
        p => new OrderPositionModel
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Amount = p.Amount,
            UnitPrice = p.UnitPrice,
            TotalValue = p.TotalValue
        };
}
