using eShop.Domain.Orders.Events;
using eShop.Domain.Orders.Rules;
using eShop.Domain.Products;
using eShop.Shared.DDD;
using eShop.Shared.DDD.Validation;

namespace eShop.Domain.Orders;

public class Order : EntityBase<long>
{
    public Guid Guid { get; set; }
    public decimal GrandTotalValue { get; set; }
    public EOrderStatus Status { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime? PaymentDateTime { get; set; }
    public DateTime? ShippingDateTime { get; set; }
    public ICollection<OrderPosition> Positions { get; set; }
    public List<IDomainEvent> Events { get; set; }
    public string UserEmail { get; set; }

    public Order()
    {
    }

    public Order(Guid orderGuid, string userEmail, string shippingAddress)
    {
        CheckRule(new GuidFieldRequiredRule(nameof(orderGuid), orderGuid));
        CheckRule(new StringFieldRequiredRule(nameof(userEmail), userEmail));
        CheckRule(new StringFieldRequiredRule(nameof(shippingAddress), shippingAddress));

        Guid = orderGuid;
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        Status = EOrderStatus.Created;
        Positions ??= [];

        AddDomainEvent(new OrderCreatedDomainEvent(Guid, userEmail, shippingAddress));
    }

    public void AddPosition(Product product, int amount)
    {
        CheckRule(new RequiredOrderStatusRule(EOrderStatus.Created, Status));
        CheckRule(new CorrectPositionRule(product, amount));
        CheckRule(new UniqueProductsRule(Positions, product.Id, product.Name));

        var positionTotal = amount * product.Price;
        var position = OrderPosition.Create(product.Id, product.Name, product.Price, amount, positionTotal);

        Positions ??= [];
        Positions.Add(position);
        CalculateGrandTotalValue();

        AddDomainEvent(new OrderPositionAddedDomainEvent(Id, product.Id, product.Name, product.Price, amount));
    }

    public void RemovePosition(long productId)
    {
        CheckRule(new RequiredOrderStatusRule(EOrderStatus.Created, Status));
        CheckRule(new PositionExistsRule(Positions, productId));

        OrderPosition toRemove = Positions.First(p => p.ProductId == productId);
        Positions.Remove(toRemove);
        CalculateGrandTotalValue();

        AddDomainEvent(new OrderPositionRemovedDomainEvent(Id, productId));
    }

    public void MarkAsPaid(DateTime payDateTime)
    {
        CheckRule(new RequiredOrderStatusRule(EOrderStatus.Created, Status));
        CheckRule(new OrderNotPaidRule(Id, PaymentDateTime));
        CheckRule(new MustContainPositionRule(Id, Positions));

        PaymentDateTime = payDateTime;
        Status = EOrderStatus.Paid;

        AddDomainEvent(new OrderPaidDomainEvent(Id, payDateTime));
    }

    public void MarkAsShipped(DateTime shippingDateTime)
    {
        CheckRule(new RequiredOrderStatusRule(EOrderStatus.Paid, Status));
        CheckRule(new OrderPaidRule(Id, PaymentDateTime));

        ShippingDateTime = shippingDateTime;
        Status = EOrderStatus.Shipped;

        AddDomainEvent(new OrderShippedDomainEvent(Id, shippingDateTime));
    }

    private void CalculateGrandTotalValue()
    {
        GrandTotalValue = Positions
            .Select(p => p.UnitPrice * p.Amount)
            .Sum();
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        Events ??= [];
        Events.Add(domainEvent);
    }

    private static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleException(rule);
        }
    }

    protected static bool IsSatisfied(IBusinessRule rule) => !rule.IsBroken();
}
