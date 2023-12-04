using Convey.CQRS.Commands;
using eShop.Domain.Orders.Exceptions;
using eShop.Infrastructure.EntityFramework;
using eShop.Shared.Emailing;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Orders.Commands.Handlers;

public class OrderPaidNotifierCommandHandler : ICommandHandler<OrderPaidNotifierCommand>
{
    private readonly IEmailSender _emailSender;
    private readonly EShopDbContext _dbContext;

    public OrderPaidNotifierCommandHandler(IEmailSender emailSender, EShopDbContext dbContext)
    {
        _emailSender = emailSender;
        _dbContext = dbContext;
    }

    public async Task HandleAsync(OrderPaidNotifierCommand command, CancellationToken ct)
    {
        string userEmail = await _dbContext.Orders
            .Where(x => x.Guid == command.OrderGuid)
            .Select(x => x.UserEmail)
            .FirstOrDefaultAsync(ct);
        
        await _emailSender.SendAsync(
            userEmail,
            $"The order {command.OrderGuid} has been paid on {command.PaymentDateTime.ToHumanReadableString()}",
            "Order will be sent soon.");
    }
}