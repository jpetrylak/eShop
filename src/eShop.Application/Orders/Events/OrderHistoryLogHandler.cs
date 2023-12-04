using eShop.Domain.Orders;
using eShop.Domain.Orders.Events;
using eShop.Domain.Orders.Exceptions;
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
        long orderId = await GetOrderIdAsync(@event.OrderGuid);
        var message = $"Order paid on {@event.PaymentDateTime.ToHumanReadableString()}";
        await InsertLogAsync(orderId, message);
    }

    public async Task HandleAsync(OrderPositionAddedDomainEvent @event)
    {
        long orderId = await GetOrderIdAsync(@event.OrderGuid);
        var message = $"Added position: product: {@event.ProductName}, amount: {@event.Amount}, unit price: {@event.UnitPrice}";
        await InsertLogAsync(orderId, message);
    }

    public async Task HandleAsync(OrderPositionRemovedDomainEvent @event)
    {
        long orderId = await GetOrderIdAsync(@event.OrderGuid);
        string productName = await GetProductNameAsync(@event.ProductId);
        var message = $"Removed position: {orderId}: product: {productName}";
        await InsertLogAsync(orderId, message);
    }

    public async Task HandleAsync(OrderShippedDomainEvent @event)
    {
        long orderId = await GetOrderIdAsync(@event.OrderGuid);
        var message = $"Order shipped on {@event.ShippingDateTime.ToHumanReadableString()}";
        await InsertLogAsync(orderId, message);
    }

    private async Task InsertLogAsync(long orderId, string message)
    {
        var log = new OrderHistoryLog(orderId, message);
        
        await _context.OrderHistoryLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    private async Task<long> GetOrderIdAsync(Guid orderGuid) =>
        await _context.Orders
            .Where(x => x.Guid == orderGuid)
            .Select(x => x.Id)
            .FirstAsync();
    
    private async Task<string> GetProductNameAsync(long productId) =>
        await _context.Products
            .Where(x => x.Id == productId)
            .Select(x => x.Name)
            .FirstAsync();
}