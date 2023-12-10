using Convey.CQRS.Commands;
using eShop.Domain.Orders;
using eShop.Domain.Products;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using eShop.Shared.CQRS;
using eShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Commands.Handlers;

public class CreateOrderPositionCommandHandler : ICommandHandler<CreateOrderPositionCommand>
{
    private readonly EShopDbContext _dbContext;
    private readonly IDomainEventsDispatcher _eventsDispatcher;

    public CreateOrderPositionCommandHandler(EShopDbContext dbContext, IDomainEventsDispatcher eventsDispatcher)
    {
        _dbContext = dbContext;
        _eventsDispatcher = eventsDispatcher;
    }

    public async Task HandleAsync(CreateOrderPositionCommand command, CancellationToken ct = new())
    {
        Order order = await _dbContext.Orders
            .Include(o => o.Positions)
            .GetAsync(command.OrderId);

        Product product = await _dbContext.Products.GetAsync(command.ProductId);
        order.AddPosition(product, command.Amount);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}
