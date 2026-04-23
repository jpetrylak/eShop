using Lightstreamer.DotNet.Server;
using Microsoft.Extensions.Options;

namespace FxRatesProvider;

public class FxRatesProviderWorker : BackgroundService
{
    private readonly LightstreamerSdkLoggingInitializer _loggingInitializer;
    private readonly FxRatesDataAdapter _dataAdapter;
    private readonly FxRatesMetadataProvider _metadataProvider;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<FxRatesProviderWorker> _logger;
    private readonly FxRatesProviderOptions _options;

    public FxRatesProviderWorker(
        LightstreamerSdkLoggingInitializer loggingInitializer,
        FxRatesDataAdapter dataAdapter,
        FxRatesMetadataProvider metadataProvider,
        IHostApplicationLifetime applicationLifetime,
        IOptions<FxRatesProviderOptions> options,
        ILogger<FxRatesProviderWorker> logger)
    {
        _loggingInitializer = loggingInitializer;
        _dataAdapter = dataAdapter;
        _metadataProvider = metadataProvider;
        _applicationLifetime = applicationLifetime;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = _loggingInitializer;
        _options.Validate();

        bool shuttingDown = false;
        LightstreamerConnectionSet dataConnections = null;
        LightstreamerConnectionSet metadataConnections = null;
        DataProviderServer dataServer = null;
        MetadataProviderServer metadataServer = null;

        try
        {
            _logger.LogInformation(
                "Connecting FX rates data adapter to {Host}:{Port}",
                _options.Lightstreamer.DataAdapter.Host,
                _options.Lightstreamer.DataAdapter.RequestReplyPort);

            dataConnections = await LightstreamerConnectionSet.ConnectDataAdapterAsync(
                _options.Lightstreamer.DataAdapter,
                stoppingToken);

            _logger.LogInformation(
                "Connecting FX rates metadata adapter to {Host}:{Port}",
                _options.Lightstreamer.MetadataAdapter.Host,
                _options.Lightstreamer.MetadataAdapter.RequestReplyPort);

            metadataConnections = await LightstreamerConnectionSet.ConnectMetadataAdapterAsync(
                _options.Lightstreamer.MetadataAdapter,
                stoppingToken);

            dataServer = CreateDataProviderServer(
                dataConnections,
                new LightstreamerServerExceptionHandler(
                    _logger,
                    _applicationLifetime,
                    "data adapter",
                    () => shuttingDown || stoppingToken.IsCancellationRequested));

            metadataServer = CreateMetadataProviderServer(
                metadataConnections,
                new LightstreamerServerExceptionHandler(
                    _logger,
                    _applicationLifetime,
                    "metadata adapter",
                    () => shuttingDown || stoppingToken.IsCancellationRequested));

            metadataServer.Start();
            dataServer.Start();

            _logger.LogInformation(
                "Fx rates Lightstreamer adapters started for {PairCount} configured pairs. Consumers can subscribe by item name using the currency pair and schema '{SchemaName}'",
                _options.CurrencyPairs.Count,
                FxRateFields.SchemaName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            shuttingDown = true;
            _dataAdapter.StopAll();
            dataServer?.Close();
            metadataServer?.Close();
            dataConnections?.Dispose();
            metadataConnections?.Dispose();
        }
    }

    private DataProviderServer CreateDataProviderServer(
        LightstreamerConnectionSet connections,
        LightstreamerServerExceptionHandler exceptionHandler)
    {
        var server = new DataProviderServer
        {
            Adapter = _dataAdapter,
            ExceptionHandler = exceptionHandler,
            Name = "FxRatesProvider.DataAdapter",
            RequestStream = connections.RequestStream,
            ReplyStream = connections.ReplyStream
        };

        ApplyRemoteCredentials(server);
        return server;
    }

    private MetadataProviderServer CreateMetadataProviderServer(
        LightstreamerConnectionSet connections,
        LightstreamerServerExceptionHandler exceptionHandler)
    {
        var server = new MetadataProviderServer
        {
            Adapter = _metadataProvider,
            ExceptionHandler = exceptionHandler,
            Name = "FxRatesProvider.MetadataAdapter",
            RequestStream = connections.RequestStream,
            ReplyStream = connections.ReplyStream
        };

        ApplyRemoteCredentials(server);
        return server;
    }

    private void ApplyRemoteCredentials(Server server)
    {
        if (string.IsNullOrWhiteSpace(_options.Lightstreamer.RemoteUser) ||
            string.IsNullOrWhiteSpace(_options.Lightstreamer.RemotePassword))
        {
            return;
        }

        server.RemoteUser = _options.Lightstreamer.RemoteUser;
        server.RemotePassword = _options.Lightstreamer.RemotePassword;
    }
}
