using Convey.CQRS.Queries;
using eShop.Application.Orders.Models;
using eShop.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Queries.Handlers;

public class GetOrderHistoryQueryHandler : IQueryHandler<GetOrderHistoryQuery, IEnumerable<OrderHistoryModel>>
{
    private readonly EShopDbContext _context;

    public GetOrderHistoryQueryHandler(EShopDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderHistoryModel>> HandleAsync(GetOrderHistoryQuery query, CancellationToken ct)
    {
        List<OrderHistoryModel> entries = await _context.OrderHistoryLogs
            .AsNoTracking()
            .Where(x => x.OrderId == query.OrderId)
            .Select(x => new OrderHistoryModel
            {
                Message = x.Message,
                Occured = x.Occured
            })
            .ToListAsync(ct);

        return entries;
    }
}