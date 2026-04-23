using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Lightstreamer.Interfaces.Data;
using Microsoft.Extensions.Options;

namespace FxRatesProvider;

public class FxRatesDataAdapter : IDataProvider
{
    private readonly ConcurrentDictionary<string, ActivePairFeed> _activeFeeds = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, FxRateState> _lastKnownStates = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, PairDefinition> _pairsByKey;
    private readonly ILogger<FxRatesDataAdapter> _logger;
    private readonly int _minUpdateIntervalMs;
    private readonly int _maxUpdateIntervalMs;

    private IItemEventListener _listener;

    public FxRatesDataAdapter(
        IOptions<FxRatesProviderOptions> options,
        ILogger<FxRatesDataAdapter> logger)
    {
        FxRatesProviderOptions providerOptions = options.Value;
        providerOptions.Validate();

        _logger = logger;
        _minUpdateIntervalMs = providerOptions.MinUpdateIntervalMs;
        _maxUpdateIntervalMs = providerOptions.MaxUpdateIntervalMs;
        _pairsByKey = providerOptions.CurrencyPairs
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(CreatePairDefinition)
            .ToDictionary(definition => NormalizePair(definition.Pair), definition => definition, StringComparer.OrdinalIgnoreCase);

        foreach (PairDefinition definition in _pairsByKey.Values)
        {
            _lastKnownStates[definition.Pair] = CreateInitialState(definition);
        }
    }

    public void Init(IDictionary parameters, string configFile)
    {
        _logger.LogInformation(
            "Lightstreamer FX data adapter initialized for {PairCount} pairs. Config file: {ConfigFile}. Parameters: {ParameterCount}",
            _pairsByKey.Count,
            configFile ?? "<none>",
            parameters?.Count ?? 0);
    }

    public bool IsSnapshotAvailable(string itemName)
    {
        EnsurePair(itemName);
        return true;
    }

    public void SetListener(IItemEventListener eventListener)
    {
        _listener = eventListener ?? throw new ArgumentNullException(nameof(eventListener));
        _logger.LogDebug("Lightstreamer FX data adapter listener attached.");
    }

    public void Subscribe(string itemName)
    {
        EnsureListener();

        PairDefinition pair = EnsurePair(itemName);
        _logger.LogInformation("Lightstreamer subscribe received for {CurrencyPair}", pair.Pair);
        _activeFeeds.GetOrAdd(pair.Pair, StartFeed);
    }

    public void Unsubscribe(string itemName)
    {
        PairDefinition pair = EnsurePair(itemName);
        _logger.LogInformation("Lightstreamer unsubscribe received for {CurrencyPair}", pair.Pair);
        if (!_activeFeeds.TryRemove(pair.Pair, out ActivePairFeed feed))
        {
            return;
        }

        feed.Subscription.Dispose();

        _logger.LogInformation("Stopped publishing {CurrencyPair}", pair.Pair);
    }

    public void StopAll()
    {
        foreach (KeyValuePair<string, ActivePairFeed> feedEntry in _activeFeeds.ToArray())
        {
            Unsubscribe(feedEntry.Key);
        }
    }

    private ActivePairFeed StartFeed(string pair)
    {
        PairDefinition definition = EnsurePair(pair);
        bool isSnapshot = true;
        IDisposable subscription = CreateRateStream(definition)
            .Subscribe(
                state =>
                {
                    _lastKnownStates[definition.Pair] = state;
                    Publish(definition, state, isSnapshot);

                    if (!isSnapshot)
                    {
                        return;
                    }

                    _listener.EndOfSnapshot(definition.Pair);
                    isSnapshot = false;
                },
                exception => _listener?.Failure(exception));

        _logger.LogInformation("Started publishing {CurrencyPair}", definition.Pair);

        return new ActivePairFeed(subscription);
    }

    private IObservable<FxRateState> CreateRateStream(PairDefinition definition)
    {
        return Observable.Defer(() =>
        {
            FxRateState state = _lastKnownStates[definition.Pair];
            FxRateState firstUpdate = CreateNextState(definition, state);

            return Observable.Return(state)
                .Concat(
                    Observable.Generate(
                        firstUpdate,
                        _ => true,
                        previous => CreateNextState(definition, previous),
                        current => current,
                        _ => NextDelay(),
                        Scheduler.Default));
        });
    }

    private void Publish(PairDefinition definition, FxRateState state, bool isSnapshot)
    {
        _logger.LogDebug(
            "Publishing {UpdateKind} update for {CurrencyPair}: bid={Bid} ask={Ask} seq={Sequence}",
            isSnapshot ? "snapshot" : "live",
            definition.Pair,
            state.Bid,
            state.Ask,
            state.Sequence);

        Hashtable payload = new()
        {
            [FxRateFields.Pair] = definition.Pair,
            [FxRateFields.Bid] = FormatPrice(state.Bid, definition.DecimalPlaces),
            [FxRateFields.Ask] = FormatPrice(state.Ask, definition.DecimalPlaces),
            [FxRateFields.Mid] = FormatPrice(state.Mid, definition.DecimalPlaces),
            [FxRateFields.Spread] = FormatPrice(state.Spread, definition.DecimalPlaces),
            [FxRateFields.Timestamp] = state.TimestampUtc.ToString("O", CultureInfo.InvariantCulture),
            [FxRateFields.Sequence] = state.Sequence.ToString(CultureInfo.InvariantCulture)
        };

        _listener.Update(definition.Pair, payload, isSnapshot);
    }

    private PairDefinition EnsurePair(string itemName)
    {
        if (_pairsByKey.TryGetValue(NormalizePair(itemName), out PairDefinition definition))
        {
            return definition;
        }

        throw new SubscriptionException($"Currency pair '{itemName}' is not configured.");
    }

    private void EnsureListener()
    {
        if (_listener is null)
        {
            throw new InvalidOperationException("The Lightstreamer item listener has not been initialized.");
        }
    }

    private static PairDefinition CreatePairDefinition(string pair)
    {
        int decimalPlaces = pair.EndsWith("/JPY", StringComparison.OrdinalIgnoreCase) ? 3 : 5;
        decimal baseMid = decimalPlaces == 3
            ? 100m + (decimal)Random.Shared.NextDouble() * 60m
            : 0.70000m + (decimal)Random.Shared.NextDouble() * 0.80000m;

        decimal baseSpread = decimalPlaces == 3 ? 0.020m : 0.00020m;
        decimal maxMove = decimalPlaces == 3 ? 0.150m : 0.00150m;

        return new PairDefinition(
            pair,
            decimalPlaces,
            decimal.Round(baseMid, decimalPlaces),
            baseSpread,
            maxMove);
    }

    private static FxRateState CreateInitialState(PairDefinition definition)
    {
        decimal bid = decimal.Round(definition.BaseMid - definition.BaseSpread / 2m, definition.DecimalPlaces);
        decimal ask = decimal.Round(definition.BaseMid + definition.BaseSpread / 2m, definition.DecimalPlaces);

        return new FxRateState(
            bid,
            ask,
            definition.BaseMid,
            definition.BaseSpread,
            DateTimeOffset.UtcNow,
            1);
    }

    private static FxRateState CreateNextState(PairDefinition definition, FxRateState previous)
    {
        decimal movement = ((decimal)Random.Shared.NextDouble() - 0.5m) * 2m * definition.MaxMove;
        decimal mid = decimal.Round(Math.Max(0.00001m, previous.Mid + movement), definition.DecimalPlaces);

        decimal spreadVariation = ((decimal)Random.Shared.NextDouble() - 0.5m) * definition.BaseSpread;
        decimal spread = decimal.Round(
            Math.Max(definition.BaseSpread / 2m, definition.BaseSpread + spreadVariation),
            definition.DecimalPlaces);

        decimal bid = decimal.Round(mid - spread / 2m, definition.DecimalPlaces);
        decimal ask = decimal.Round(mid + spread / 2m, definition.DecimalPlaces);

        return new FxRateState(
            bid,
            ask,
            mid,
            spread,
            DateTimeOffset.UtcNow,
            previous.Sequence + 1);
    }

    private static string NormalizePair(string pair)
    {
        return pair.Trim().ToUpperInvariant();
    }

    private static string FormatPrice(decimal value, int decimalPlaces)
    {
        return value.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
    }

    private TimeSpan NextDelay()
    {
        int delayMs = Random.Shared.Next(_minUpdateIntervalMs, _maxUpdateIntervalMs + 1);
        return TimeSpan.FromMilliseconds(delayMs);
    }

    private sealed record ActivePairFeed(IDisposable Subscription);

    private sealed record PairDefinition(
        string Pair,
        int DecimalPlaces,
        decimal BaseMid,
        decimal BaseSpread,
        decimal MaxMove);

    private sealed record FxRateState(
        decimal Bid,
        decimal Ask,
        decimal Mid,
        decimal Spread,
        DateTimeOffset TimestampUtc,
        long Sequence);
}
