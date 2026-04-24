namespace FxRatesProvider;

public class FxRatesProviderOptions
{
    public IList<string> CurrencyPairs { get; set; } = new List<string>();
    public int MinUpdateIntervalMs { get; set; } = 250;
    public int MaxUpdateIntervalMs { get; set; } = 1000;
    public LightstreamerOptions Lightstreamer { get; set; } = new();

    public void Validate()
    {
        if (CurrencyPairs is null || CurrencyPairs.Count == 0)
        {
            throw new InvalidOperationException("At least one currency pair must be configured.");
        }

        if (MinUpdateIntervalMs <= 0)
        {
            throw new InvalidOperationException("MinUpdateIntervalMs must be greater than zero.");
        }

        if (MaxUpdateIntervalMs < MinUpdateIntervalMs)
        {
            throw new InvalidOperationException("MaxUpdateIntervalMs must be greater than or equal to MinUpdateIntervalMs.");
        }

        Lightstreamer.Validate();
    }
}

public class LightstreamerOptions
{
    public DataAdapterConnectionOptions DataAdapter { get; set; } = new();
    public MetadataAdapterConnectionOptions MetadataAdapter { get; set; } = new();
    public string RemoteUser { get; set; } = string.Empty;
    public string RemotePassword { get; set; } = string.Empty;

    public void Validate()
    {
        DataAdapter.Validate(nameof(DataAdapter));
        MetadataAdapter.Validate(nameof(MetadataAdapter));
    }
}

public class DataAdapterConnectionOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int RequestReplyPort { get; set; }

    public void Validate(string sectionName)
    {
        if (string.IsNullOrWhiteSpace(Host))
        {
            throw new InvalidOperationException($"{sectionName}.Host must be configured.");
        }

        if (RequestReplyPort <= 0)
        {
            throw new InvalidOperationException($"{sectionName}.RequestReplyPort must be greater than zero.");
        }
    }
}

public class MetadataAdapterConnectionOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int RequestReplyPort { get; set; }

    public void Validate(string sectionName)
    {
        if (string.IsNullOrWhiteSpace(Host))
        {
            throw new InvalidOperationException($"{sectionName}.Host must be configured.");
        }

        if (RequestReplyPort <= 0)
        {
            throw new InvalidOperationException($"{sectionName}.RequestReplyPort must be greater than zero.");
        }
    }
}
