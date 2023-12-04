using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderPaidNotifierCommand(Guid OrderGuid, DateTime PaymentDateTime)
    : ICommand;