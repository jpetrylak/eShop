using Convey.CQRS.Commands;
using eShop.Domain.Orders;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Commands.Handlers;

public class CreateOrderQueryHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly EShopDbContext _dbContext;
    private readonly IDomainEventsDispatcher _eventsDispatcher;

    public CreateOrderQueryHandler(EShopDbContext dbContext, IDomainEventsDispatcher eventsDispatcher)
    {
        _dbContext = dbContext;
        _eventsDispatcher = eventsDispatcher;
    }

    public async Task HandleAsync(CreateOrderCommand command, CancellationToken ct)
    {
        var order = new Order(command.OrderGuid, command.UserEmail, command.ShippingAddress);

        await _dbContext.Orders.AddAsync(order, ct);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}