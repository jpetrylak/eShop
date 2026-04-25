using com.lightstreamer.client;

namespace FxRatesProvider.IntegrationTests.Infrastructure;

internal sealed class FxRateSubscriptionListener : SubscriptionListenerAdapter
{
    private readonly TaskCompletionSource<FxRateUpdate> _snapshotReceived =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly TaskCompletionSource<FxRateUpdate> _liveUpdateReceived =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly TaskCompletionSource<SubscriptionError> _subscriptionErrorReceived =
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    public Task<FxRateUpdate> SnapshotReceived => _snapshotReceived.Task;

    public Task<FxRateUpdate> LiveUpdateReceived => _liveUpdateReceived.Task;

    public Task<SubscriptionError> SubscriptionErrorReceived => _subscriptionErrorReceived.Task;

    public override void onItemUpdate(ItemUpdate itemUpdate)
    {
        FxRateUpdate update = FxRateUpdate.From(itemUpdate);

        if (update.Snapshot)
        {
            _snapshotReceived.TrySetResult(update);
            return;
        }

        _liveUpdateReceived.TrySetResult(update);
    }

    public override void onSubscriptionError(int code, string message)
    {
        _subscriptionErrorReceived.TrySetResult(new SubscriptionError(code, message));
    }
}
