using Convey.CQRS.Commands;

namespace eShop.Application.Orders.Commands;

public record OrderPaidNotifierCommand(long OrderId, DateTime PaymentDateTime) : ICommand;
