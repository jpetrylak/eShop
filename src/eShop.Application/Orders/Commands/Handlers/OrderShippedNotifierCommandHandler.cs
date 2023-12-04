using Convey.CQRS.Commands;
using eShop.Domain.Orders.Exceptions;
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
            .Where(x => x.Guid == command.OrderGuid)
            .Select(x => x.UserEmail)
            .FirstOrDefaultAsync(ct);
        
        await _emailSender.SendAsync(
            userEmail,
            $"Order {command.OrderGuid} has been shipped on {command.ShippingDateTime.ToHumanReadableString()}",
            "Order will be delivered soon.");
    }
}