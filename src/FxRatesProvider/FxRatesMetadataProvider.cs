using Lightstreamer.Interfaces.Metadata;
using Microsoft.Extensions.Options;

namespace FxRatesProvider;

public class FxRatesMetadataProvider : MetadataProviderAdapter
{
    private readonly Dictionary<string, string> _pairsByKey;
    private readonly Dictionary<string, string> _fieldsByKey;
    private readonly ILogger<FxRatesMetadataProvider> _logger;

    public FxRatesMetadataProvider(
        IOptions<FxRatesProviderOptions> options,
        ILogger<FxRatesMetadataProvider> logger)
    {
        _logger = logger;
        _pairsByKey = options.Value.CurrencyPairs
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToDictionary(NormalizePair, pair => pair, StringComparer.OrdinalIgnoreCase);

        _fieldsByKey = FxRateFields.All.ToDictionary(field => field, field => field, StringComparer.OrdinalIgnoreCase);
    }

    public override string[] GetItems(string user, string sessionID, string group)
    {
        _logger.LogDebug(
            "Resolving Lightstreamer items for user '{User}' session '{SessionId}' and group '{Group}'",
            user ?? "<anonymous>",
            sessionID ?? "<none>",
            group ?? "<null>");

        if (string.IsNullOrWhiteSpace(group))
        {
            throw new ItemsException("A currency pair item list is required.");
        }

        string[] requestedItems = group.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (requestedItems.Length == 0)
        {
            throw new ItemsException("A currency pair item list is required.");
        }

        string[] resolvedItems = new string[requestedItems.Length];
        for (int index = 0; index < requestedItems.Length; index++)
        {
            if (!_pairsByKey.TryGetValue(NormalizePair(requestedItems[index]), out string pair))
            {
                throw new ItemsException($"Currency pair '{requestedItems[index]}' is not configured.");
            }

            resolvedItems[index] = pair;
        }

        return resolvedItems;
    }

    public override string[] GetSchema(string user, string sessionID, string group, string schema)
    {
        _logger.LogDebug(
            "Resolving Lightstreamer schema for user '{User}' session '{SessionId}', group '{Group}', schema '{Schema}'",
            user ?? "<anonymous>",
            sessionID ?? "<none>",
            group ?? "<null>",
            schema ?? "<null>");

        if (string.IsNullOrWhiteSpace(schema) ||
            schema.Equals(FxRateFields.SchemaName, StringComparison.OrdinalIgnoreCase))
        {
            return FxRateFields.All;
        }

        string[] requestedFields = schema.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (requestedFields.Length == 0)
        {
            return FxRateFields.All;
        }

        string[] resolvedFields = new string[requestedFields.Length];
        for (int index = 0; index < requestedFields.Length; index++)
        {
            if (!_fieldsByKey.TryGetValue(requestedFields[index], out string field))
            {
                throw new SchemaException($"Field '{requestedFields[index]}' is not supported.");
            }

            resolvedFields[index] = field;
        }

        return resolvedFields;
    }

    public override bool IsModeAllowed(string user, string item, Mode mode)
    {
        _logger.LogDebug(
            "Checking mode '{Mode}' for user '{User}' item '{Item}'",
            mode,
            user ?? "<anonymous>",
            item ?? "<null>");

        return mode == Mode.MERGE;
    }

    private static string NormalizePair(string pair)
    {
        return pair.Trim().ToUpperInvariant();
    }
}
