using Convey.CQRS.Commands;
using eShop.Shared.Emailing;

namespace eShop.Application.Orders.Commands.Handlers;

public class OrderCreatedNotifierCommandHandler : ICommandHandler<OrderCreatedNotifierCommand>
{
    private readonly IEmailSender _emailSender;

    public OrderCreatedNotifierCommandHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task HandleAsync(OrderCreatedNotifierCommand command, CancellationToken ct)
    {
        await _emailSender.SendAsync(
            command.UserEmail,
            $"The order {command.OrderGuid} has been created",
            "Order is waiting for payment.");
    }
}