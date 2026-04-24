using Convey.CQRS.Commands;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Commands.Handlers;

public class ShipOrderCommandHandler : ICommandHandler<ShipOrderCommand>
{
    private readonly EShopDbContext _dbContext;
    private readonly IDomainEventsDispatcher _eventsDispatcher;

    public ShipOrderCommandHandler(EShopDbContext dbContext, IDomainEventsDispatcher eventsDispatcher)
    {
        _dbContext = dbContext;
        _eventsDispatcher = eventsDispatcher;
    }

    public async Task HandleAsync(ShipOrderCommand command, CancellationToken ct)
    {
        Order order = await _dbContext.Orders.GetAsync(command.OrderId);
        order.MarkAsShipped(DateTime.Now);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}