using com.lightstreamer.client;

namespace FxRatesProvider.IntegrationTests.Infrastructure;

internal abstract class SubscriptionListenerAdapter : SubscriptionListener
{
    public virtual void onClearSnapshot(string itemName, int itemPos)
    {
    }

    public virtual void onCommandSecondLevelItemLostUpdates(int lostUpdates, string key)
    {
    }

    public virtual void onCommandSecondLevelSubscriptionError(int code, string message, string key)
    {
    }

    public virtual void onEndOfSnapshot(string itemName, int itemPos)
    {
    }

    public virtual void onItemLostUpdates(string itemName, int itemPos, int lostUpdates)
    {
    }

    public virtual void onItemUpdate(ItemUpdate itemUpdate)
    {
    }

    public virtual void onListenEnd()
    {
    }

    public virtual void onListenStart()
    {
    }

    public virtual void onRealMaxFrequency(string frequency)
    {
    }

    public virtual void onSubscription()
    {
    }

    public virtual void onSubscriptionError(int code, string message)
    {
    }

    public virtual void onUnsubscription()
    {
    }
}
