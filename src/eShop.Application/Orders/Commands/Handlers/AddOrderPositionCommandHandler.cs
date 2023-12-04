using Convey.CQRS.Commands;
using eShop.Domain.Orders;
using eShop.Domain.Products;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using eShop.Shared.CQRS;

namespace eShop.Application.Orders.Commands.Handlers;

public class AddOrderPositionCommandHandler : ICommandHandler<AddOrderPositionCommand>
{
    private readonly EShopDbContext _dbContext;
    private readonly IDomainEventsDispatcher _eventsDispatcher;

    public AddOrderPositionCommandHandler(EShopDbContext dbContext, IDomainEventsDispatcher eventsDispatcher)
    {
        _dbContext = dbContext;
        _eventsDispatcher = eventsDispatcher;
    }

    public async Task HandleAsync(AddOrderPositionCommand command, CancellationToken ct)
    {
        Order order = await _dbContext.Orders.GetAsync(command.OrderId);
        Product product = await _dbContext.Products.GetAsync(command.ProductId);
        order.AddPosition(product, command.Amount);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}