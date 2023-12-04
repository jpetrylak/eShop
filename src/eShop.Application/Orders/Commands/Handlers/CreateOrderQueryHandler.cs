using Convey.CQRS.Commands;
using eShop.Application.Orders.Models;
using eShop.Domain.Orders;
using eShop.Domain.Products;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
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
            
        foreach (CreateOrderPositionModel position in command.Positions)
        {
            Product product = await _dbContext.Products.GetAsync(position.ProductId);
                    
            order.AddPosition(product, position.Amount);
        }

        await _dbContext.Orders.AddAsync(order, ct);
        await _dbContext.SaveChangesAsync(ct);
        await _eventsDispatcher.DispatchAsync(order.Events);
    }
}