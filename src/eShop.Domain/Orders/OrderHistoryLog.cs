using eShop.Shared.DDD;

namespace eShop.Domain.Orders;

public class OrderHistoryLog : EntityBase<long>
{
    public const int MessageMaxLength = 1000;
    public string Message { get; set; }
    public DateTime Occured { get; set; }
    public virtual Order Order { get; set; }
    public long OrderId { get; set; }

    public OrderHistoryLog()
    {
    }
    
    public OrderHistoryLog(long orderId, string message)
    {
        OrderId = orderId;
        Message = message;
        Occured = DateTime.Now;
    }
}