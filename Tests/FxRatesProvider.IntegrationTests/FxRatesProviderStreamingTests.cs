namespace FxRatesProvider.IntegrationTests;

public class FxRatesProviderStreamingTests : IClassFixture<FxRatesProviderFixture>
{
    private const string UnknownCurrencyPair = "XYZ/ABC";
    private static readonly TimeSpan UpdateTimeout = TimeSpan.FromSeconds(15);
    private readonly FxRatesProviderFixture _fixture;

    public FxRatesProviderStreamingTests(FxRatesProviderFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Subscribe_ConfiguredCurrencyPair_ReceivesSnapshotAndLiveRates()
    {
        string pair = _fixture.CurrencyPairs[0];
        await using FxRateSubscriptionClient client = CreateClient();
        FxRateSubscriptionListener listener = client.SubscribeTo(pair);

        client.Connect();

        FxRateUpdate snapshot = await listener.SnapshotReceived.WaitAsync(UpdateTimeout);
        FxRateUpdate liveUpdate = await listener.LiveUpdateReceived.WaitAsync(UpdateTimeout);

        snapshot.ShouldBeValidSnapshot(pair);
        liveUpdate.ShouldBeValidLiveUpdateAfter(snapshot);
    }

    [Fact]
    public async Task Subscribe_UnknownCurrencyPair_ReceivesSubscriptionError()
    {
        await using FxRateSubscriptionClient client = CreateClient();
        FxRateSubscriptionListener listener = client.SubscribeTo(UnknownCurrencyPair);

        client.Connect();

        SubscriptionError error = await listener.SubscriptionErrorReceived.WaitAsync(UpdateTimeout);

        error.ShouldBeValidSubscriptionError();
    }

    private FxRateSubscriptionClient CreateClient()
    {
        return new FxRateSubscriptionClient(_fixture.ServerAddress, _fixture.AdapterSet);
    }
}
