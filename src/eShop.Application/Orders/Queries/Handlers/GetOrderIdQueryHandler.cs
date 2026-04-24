using Convey.CQRS.Queries;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetOrderIdQueryHandler : IQueryHandler<GetOrderIdQuery, long>
{
    private readonly EShopDbContext _dbContext;

    public GetOrderIdQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> HandleAsync(GetOrderIdQuery query, CancellationToken ct)
    {
        long orderId = await _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.Guid == query.OrderGuid)
            .Select(o => o.Id)
            .FirstOrDefaultAsync(ct);

        if (orderId == default)
        {
            throw new EntityNotFoundException(query.OrderGuid, typeof(Order));
        }

        return orderId;
    }
}