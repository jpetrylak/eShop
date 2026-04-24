namespace FxRatesProvider;

public static class FxRateFields
{
    public const string SchemaName = "fxrates";
    public const string Pair = "pair";
    public const string Bid = "bid";
    public const string Ask = "ask";
    public const string Mid = "mid";
    public const string Spread = "spread";
    public const string Timestamp = "timestamp";
    public const string Sequence = "sequence";

    public static readonly string[] All =
    [
        Pair,
        Bid,
        Ask,
        Mid,
        Spread,
        Timestamp,
        Sequence
    ];
}
