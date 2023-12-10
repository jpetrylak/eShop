using Convey.CQRS.Commands;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using eShop.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Commands.Handlers;

public class PayForOrderCommandHandler : ICommandHandler<PayForOrderCommand>
{
    private readonly EShopDbContext _dbContext;
    private readonly IDomainEventsDispatcher _eventsDispatcher;

    public PayForOrderCommandHandler(EShopDbContext dbContext, IDomainEventsDispatcher eventsDispatcher)
    {
        _dbContext = dbContext;
        _eventsDispatcher = eventsDispatcher;
    }

    public async Task HandleAsync(PayForOrderCommand command, CancellationToken ct)
    {
        Order order = await _dbContext.Orders
            .Include(o => o.Positions)
            .GetAsync(command.OrderId);

        order.MarkAsPaid(DateTime.Now);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}
