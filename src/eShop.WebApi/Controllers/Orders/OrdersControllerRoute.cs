namespace eShop.Controllers.Orders;

public static class OrdersControllerRoute
{
    public const string Orders = nameof(Orders);

    public const string GetOrder = Orders + nameof(GetOrder);
    public const string GetOrders = Orders + nameof(GetOrders);
    public const string GetPositions = Orders + nameof(GetPositions);
    public const string GetHistory = Orders + nameof(GetHistory);
    public const string CreateOrder = Orders + nameof(CreateOrder);
    public const string CreatePosition = Orders + nameof(CreatePosition);
    public const string DeletePosition = Orders + nameof(DeletePosition);
    public const string PayOrder = Orders + nameof(PayOrder);
    public const string ShipOrder = Orders + nameof(ShipOrder);
}
