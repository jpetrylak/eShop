namespace FxRatesProvider.IntegrationTests.Assertions;

internal static class SubscriptionErrorAssertions
{
    public static void ShouldBeValidSubscriptionError(this SubscriptionError error)
    {
        error.Code.Should().NotBe(0);
        error.Message.Should().NotBeNullOrWhiteSpace();
    }
}
