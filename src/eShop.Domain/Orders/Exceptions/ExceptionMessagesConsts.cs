namespace eShop.Domain.Orders.Exceptions;

public static class ExceptionMessagesConsts
{
    public const string FieldRequiredMessage = "Field {0} is required";
    public const string InvalidOrderStatusMessage = "Expected order status is {0} but current status is {1}";
    public const string InvalidPositionMessage = "Incorrect order item for the product {1} (id {0})";
    public const string OrderNotPaidMessage = "The order {0} has not been paid yet";
    public const string ProductAlreadyAddedMessage = "There is already a position with the product {0}";
    public const string OrderDoesNotContainPositionMessage = "The order {0} does not contain positions";
    public const string OrderAlreadyPaidMessage = "Order {0} was already paid on {1}";
    public const string PositionDoesNotExistsMessage = "The selected product does not exist in the order";

    public static string ToHumanReadableString(this DateTime dt) => dt.ToString("dd.MM.yyyy hh:mm");
}
