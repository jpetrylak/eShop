using com.lightstreamer.client;
using System.Globalization;

namespace FxRatesProvider.IntegrationTests;

public class FxRatesProviderStreamingTests : IClassFixture<FxRatesProviderFixture>
{
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
        LightstreamerClient client = new(_fixture.ServerAddress, _fixture.AdapterSet);
        RateSubscriptionListener listener = new();
        Subscription subscription = CreateSubscription(pair);
        subscription.addListener(listener);

        try
        {
            client.subscribe(subscription);
            client.connect();

            FxRateUpdate snapshot = await listener.SnapshotReceived.Task.WaitAsync(UpdateTimeout);
            FxRateUpdate liveUpdate = await listener.LiveUpdateReceived.Task.WaitAsync(UpdateTimeout);

            snapshot.Pair.Should().Be(pair);
            snapshot.Snapshot.Should().BeTrue();
            snapshot.Sequence.Should().Be(1);
            snapshot.TimestampUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));

            liveUpdate.Pair.Should().Be(pair);
            liveUpdate.Snapshot.Should().BeFalse();
            liveUpdate.Sequence.Should().BeGreaterThan(snapshot.Sequence);
            liveUpdate.Bid.Should().BePositive();
            liveUpdate.Ask.Should().BeGreaterThan(liveUpdate.Bid);
            liveUpdate.Mid.Should().BePositive();
            liveUpdate.Spread.Should().BePositive();
            liveUpdate.TimestampUtc.Should().BeAfter(snapshot.TimestampUtc.AddMilliseconds(-1));
        }
        finally
        {
            client.unsubscribe(subscription);
            client.disconnect();
        }
    }

    [Fact]
    public async Task Subscribe_UnknownCurrencyPair_ReceivesSubscriptionError()
    {
        LightstreamerClient client = new(_fixture.ServerAddress, _fixture.AdapterSet);
        RateSubscriptionListener listener = new();
        Subscription subscription = CreateSubscription("XYZ/ABC");
        subscription.addListener(listener);

        try
        {
            client.subscribe(subscription);
            client.connect();

            SubscriptionError error = await listener.SubscriptionErrorReceived.Task.WaitAsync(UpdateTimeout);

            error.Code.Should().NotBe(0);
            error.Message.Should().NotBeNullOrWhiteSpace();
        }
        finally
        {
            client.unsubscribe(subscription);
            client.disconnect();
        }
    }

    private static Subscription CreateSubscription(string pair)
    {
        Subscription subscription = new("MERGE", pair, FxRateFields.All);
        subscription.RequestedSnapshot = "yes";
        return subscription;
    }

    private sealed class RateSubscriptionListener : SubscriptionListener
    {
        public TaskCompletionSource<FxRateUpdate> SnapshotReceived { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource<FxRateUpdate> LiveUpdateReceived { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource<SubscriptionError> SubscriptionErrorReceived { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void onClearSnapshot(string itemName, int itemPos)
        {
        }

        public void onCommandSecondLevelItemLostUpdates(int lostUpdates, string key)
        {
        }

        public void onCommandSecondLevelSubscriptionError(int code, string message, string key)
        {
        }

        public void onEndOfSnapshot(string itemName, int itemPos)
        {
        }

        public void onItemLostUpdates(string itemName, int itemPos, int lostUpdates)
        {
        }

        public void onItemUpdate(ItemUpdate itemUpdate)
        {
            FxRateUpdate update = FxRateUpdate.From(itemUpdate);

            if (update.Snapshot)
            {
                SnapshotReceived.TrySetResult(update);
                return;
            }

            LiveUpdateReceived.TrySetResult(update);
        }

        public void onListenEnd()
        {
        }

        public void onListenStart()
        {
        }

        public void onRealMaxFrequency(string frequency)
        {
        }

        public void onSubscription()
        {
        }

        public void onSubscriptionError(int code, string message)
        {
            SubscriptionErrorReceived.TrySetResult(new SubscriptionError(code, message));
        }

        public void onUnsubscription()
        {
        }
    }

    private sealed record FxRateUpdate(
        bool Snapshot,
        string Pair,
        decimal Bid,
        decimal Ask,
        decimal Mid,
        decimal Spread,
        DateTimeOffset TimestampUtc,
        long Sequence)
    {
        public static FxRateUpdate From(ItemUpdate itemUpdate)
        {
            return new FxRateUpdate(
                itemUpdate.Snapshot,
                itemUpdate.getValue(FxRateFields.Pair),
                decimal.Parse(itemUpdate.getValue(FxRateFields.Bid), CultureInfo.InvariantCulture),
                decimal.Parse(itemUpdate.getValue(FxRateFields.Ask), CultureInfo.InvariantCulture),
                decimal.Parse(itemUpdate.getValue(FxRateFields.Mid), CultureInfo.InvariantCulture),
                decimal.Parse(itemUpdate.getValue(FxRateFields.Spread), CultureInfo.InvariantCulture),
                DateTimeOffset.Parse(itemUpdate.getValue(FxRateFields.Timestamp), CultureInfo.InvariantCulture),
                long.Parse(itemUpdate.getValue(FxRateFields.Sequence), CultureInfo.InvariantCulture));
        }
    }

    private sealed record SubscriptionError(int Code, string Message);
}
