namespace FxRatesProvider.IntegrationTests.Assertions;

internal static class FxRateUpdateAssertions
{
    private static readonly TimeSpan TimestampTolerance = TimeSpan.FromMinutes(1);

    public static void ShouldBeValidSnapshot(this FxRateUpdate snapshot, string expectedPair)
    {
        snapshot.Pair.Should().Be(expectedPair);
        snapshot.Snapshot.Should().BeTrue();
        snapshot.Sequence.Should().Be(1);
        snapshot.TimestampUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimestampTolerance);
    }

    public static void ShouldBeValidLiveUpdateAfter(this FxRateUpdate liveUpdate, FxRateUpdate snapshot)
    {
        liveUpdate.Pair.Should().Be(snapshot.Pair);
        liveUpdate.Snapshot.Should().BeFalse();
        liveUpdate.Sequence.Should().BeGreaterThan(snapshot.Sequence);
        liveUpdate.Bid.Should().BePositive();
        liveUpdate.Ask.Should().BeGreaterThan(liveUpdate.Bid);
        liveUpdate.Mid.Should().BePositive();
        liveUpdate.Spread.Should().BePositive();
        liveUpdate.TimestampUtc.Should().BeAfter(snapshot.TimestampUtc.AddMilliseconds(-1));
    }
}
