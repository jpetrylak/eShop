using Convey.CQRS.Commands;
using eShop.Domain.Orders.Rules;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.Emailing;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Commands.Handlers;

public class OrderShippedNotifierCommandHandler : ICommandHandler<OrderShippedNotifierCommand>
{
    private readonly IEmailSender _emailSender;
    private readonly EShopDbContext _dbContext;

    public OrderShippedNotifierCommandHandler(IEmailSender emailSender, EShopDbContext dbContext)
    {
        _emailSender = emailSender;
        _dbContext = dbContext;
    }

    public async Task HandleAsync(OrderShippedNotifierCommand command, CancellationToken ct)
    {
        string userEmail = await _dbContext.Orders
            .Where(o => o.Id == command.OrderId)
            .Select(o => o.UserEmail)
            .FirstOrDefaultAsync(ct);
        
        await _emailSender.SendAsync(
            userEmail,
            $"Order {command.OrderId} has been shipped on {command.ShippingDateTime.ToHumanReadableString()}",
            "Order will be delivered soon.");
    }
}