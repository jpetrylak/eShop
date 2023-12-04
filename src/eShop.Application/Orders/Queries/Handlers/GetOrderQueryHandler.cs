using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderModel>
{
    private readonly EShopDbContext _dbContext;

    public GetOrderQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderModel> HandleAsync(GetOrderQuery query, CancellationToken ct)
    {
        Order order = await _dbContext.Orders.AsNoTracking().GetAsync(query.OrderId);

        return OrderMapper.Map(order);
    }
}