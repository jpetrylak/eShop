using com.lightstreamer.client;

namespace FxRatesProvider.IntegrationTests.Infrastructure;

internal sealed class FxRateSubscriptionClient : IAsyncDisposable
{
    private const string SubscriptionMode = "MERGE";
    private const string RequestedSnapshotValue = "yes";

    private readonly LightstreamerClient _client;
    private readonly List<Subscription> _subscriptions = [];

    public FxRateSubscriptionClient(string serverAddress, string adapterSet)
    {
        _client = new LightstreamerClient(serverAddress, adapterSet);
    }

    public FxRateSubscriptionListener SubscribeTo(string pair)
    {
        FxRateSubscriptionListener listener = new();
        Subscription subscription = new(SubscriptionMode, pair, FxRateFields.All)
        {
            RequestedSnapshot = RequestedSnapshotValue
        };

        subscription.addListener(listener);
        _client.subscribe(subscription);
        _subscriptions.Add(subscription);

        return listener;
    }

    public void Connect()
    {
        _client.connect();
    }

    public ValueTask DisposeAsync()
    {
        foreach (Subscription subscription in _subscriptions)
        {
            _client.unsubscribe(subscription);
        }

        _client.disconnect();
        return ValueTask.CompletedTask;
    }
}
