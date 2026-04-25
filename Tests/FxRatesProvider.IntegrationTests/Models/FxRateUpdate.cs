using com.lightstreamer.client;
using System.Globalization;

namespace FxRatesProvider.IntegrationTests.Models;

internal sealed record FxRateUpdate(
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
