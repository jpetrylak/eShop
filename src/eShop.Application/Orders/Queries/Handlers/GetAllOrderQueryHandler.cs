using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetAllOrderQueryHandler : IQueryHandler<GetAllOrderQuery, IEnumerable<OrderModel>>
{
    private readonly EShopDbContext _dbContext;

    public GetAllOrderQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<OrderModel>> HandleAsync(GetAllOrderQuery query, CancellationToken ct)
    {
        List<OrderModel> orders = await _dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Positions)
            .Select(x => new OrderModel
            {
                Id = x.Id,
                GrandTotalValue = x.GrandTotalValue,
                Status = x.Status,
                ShippingAddress = x.ShippingAddress,
                PaymentDateTime = x.PaymentDateTime,
                ShippingDateTime = x.ShippingDateTime
            })
            .ToListAsync(ct);

        return orders;
    }
}