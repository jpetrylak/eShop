using eShop.Domain.Orders;
using eShop.Domain.Orders.Events;
using eShop.Domain.Orders.Rules;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.DDD;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Events;

public class OrderHistoryLogHandler :
    IDomainEventHandler<OrderCreatedDomainEvent>,
    IDomainEventHandler<OrderPaidDomainEvent>,
    IDomainEventHandler<OrderPositionAddedDomainEvent>,
    IDomainEventHandler<OrderPositionRemovedDomainEvent>,
    IDomainEventHandler<OrderShippedDomainEvent>
{
    private readonly EShopDbContext _context;

    public OrderHistoryLogHandler(EShopDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(OrderCreatedDomainEvent @event)
    {
        long orderId = await GetOrderIdAsync(@event.OrderGuid);
        var message = $"Order {orderId} created";
        await InsertLogAsync(orderId, message);
    }

    public async Task HandleAsync(OrderPaidDomainEvent @event)
    {
        var message = $"Order paid on {@event.PaymentDateTime.ToHumanReadableString()}";
        await InsertLogAsync(@event.OrderId, message);
    }

    public async Task HandleAsync(OrderPositionAddedDomainEvent @event)
    {
        var message = $"Added position: product id: {@event.ProductId}, product: {@event.ProductName}, " +
                      $"amount: {@event.Amount}, unit price: {@event.UnitPrice}";
        await InsertLogAsync(@event.OrderId, message);
    }

    public async Task HandleAsync(OrderPositionRemovedDomainEvent @event)
    {
        string productName = await GetProductNameAsync(@event.ProductId);
        var message = $"Removed position: {@event.OrderId}: product: {productName}";
        await InsertLogAsync(@event.OrderId, message);
    }

    public async Task HandleAsync(OrderShippedDomainEvent @event)
    {
        var message = $"Order shipped on {@event.ShippingDateTime.ToHumanReadableString()}";
        await InsertLogAsync(@event.OrderId, message);
    }

    private async Task InsertLogAsync(long orderId, string message)
    {
        var log = new OrderHistoryLog(orderId, message);

        await _context.OrderHistoryLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    private async Task<long> GetOrderIdAsync(Guid orderGuid) =>
        await _context.Orders
            .Where(o => o.Guid == orderGuid)
            .Select(o => o.Id)
            .FirstAsync();

    private async Task<string> GetProductNameAsync(long productId) =>
        await _context.Products
            .Where(o => o.Id == productId)
            .Select(o => o.Name)
            .FirstAsync();
}
