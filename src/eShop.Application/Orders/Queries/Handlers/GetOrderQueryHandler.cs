using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderDetailsModel>
{
    private readonly EShopDbContext _dbContext;

    public GetOrderQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderDetailsModel> HandleAsync(GetOrderQuery query, CancellationToken ct)
    {
        OrderDetailsModel order = await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Positions)
            .Where(o => o.Id == query.OrderId)
            .Select(SelectExpression())
            .FirstOrDefaultAsync(ct);

        return order;
    }

    private Expression<Func<Order, OrderDetailsModel>> SelectExpression() =>
        o => new OrderDetailsModel
        {
            Id = o.Id,
            UserEmail = o.UserEmail,
            Status = o.Status,
            GrandTotalValue = o.GrandTotalValue,
            ShippingAddress = o.ShippingAddress,
            ShippingDateTime = o.ShippingDateTime,
            PaymentDateTime = o.PaymentDateTime,
            Positions = o.Positions
                .Select(p => new OrderPositionModel
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Amount = p.Amount,
                        UnitPrice = p.UnitPrice,
                        TotalValue = p.TotalValue
                    }
                )
        };
}