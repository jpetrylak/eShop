using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetPagedOrdersQueryHandler : IQueryHandler<GetPagedOrdersQuery, PagedResult<OrderModel>>
{
    private readonly EShopDbContext _dbContext;

    public GetPagedOrdersQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<OrderModel>> HandleAsync(GetPagedOrdersQuery query, CancellationToken ct)
    {
        PagedResult<OrderModel> pagedResult = await _dbContext.Orders
            .AsNoTracking()
            .Select(SelectExpression())
            .PaginateAsync(query.OrderBy, query.SortOrder, query.Page, query.Results);

        return pagedResult;
    }

    private Expression<Func<Order, OrderModel>> SelectExpression() =>
        o => new OrderModel
        {
            Id = o.Id,
            UserEmail = o.UserEmail,
            Status = o.Status,
            GrandTotalValue = o.GrandTotalValue,
            ShippingAddress = o.ShippingAddress,
            ShippingDateTime = o.ShippingDateTime,
            PaymentDateTime = o.PaymentDateTime
        };
}