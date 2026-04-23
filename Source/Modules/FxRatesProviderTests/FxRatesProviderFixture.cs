using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FxRatesProvider.IntegrationTests;

public sealed class FxRatesProviderFixture : IAsyncLifetime
{
    private const int LightstreamerHttpPort = 8080;
    private const int DataAdapterPort = 7001;
    private const int MetadataAdapterPort = 7002;
    private const string AdapterSetId = "FXRATES";

    private IContainer _container;
    private IHost _providerHost;

    public IReadOnlyList<string> CurrencyPairs { get; } = ["EUR/USD", "GBP/USD"];

    public string AdapterSet => AdapterSetId;

    public string ServerAddress
    {
        get
        {
            EnsureInitialized();
            return $"http://{_container.Hostname}:{_container.GetMappedPublicPort(LightstreamerHttpPort)}";
        }
    }

    public async Task InitializeAsync()
    {
        _container = new ContainerBuilder("lightstreamer:7.4.7")
            .WithPortBinding(LightstreamerHttpPort, true)
            .WithPortBinding(DataAdapterPort, true)
            .WithPortBinding(MetadataAdapterPort, true)
            .WithResourceMapping(
                DirectoryPath.Of(GetAdapterSetDirectory()),
                DirectoryPath.Of("/lightstreamer/adapters/FxRatesProvider/"))
            .WithWaitStrategy(
                Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(request => request.ForPort(LightstreamerHttpPort)))
            .Build();

        await _container.StartAsync();

        try
        {
            _providerHost = CreateProviderHost();
            await _providerHost.StartAsync();
        }
        catch
        {
            await _container.DisposeAsync();
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        if (_providerHost is not null)
        {
            await _providerHost.StopAsync();
            _providerHost.Dispose();
        }

        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    private IHost CreateProviderHost()
    {
        Dictionary<string, string> settings = new()
        {
            ["CurrencyPairs:0"] = CurrencyPairs[0],
            ["CurrencyPairs:1"] = CurrencyPairs[1],
            ["MinUpdateIntervalMs"] = "250",
            ["MaxUpdateIntervalMs"] = "1000",
            ["Lightstreamer:DataAdapter:Host"] = _container.Hostname,
            ["Lightstreamer:DataAdapter:RequestReplyPort"] = _container.GetMappedPublicPort(DataAdapterPort).ToString(),
            ["Lightstreamer:MetadataAdapter:Host"] = _container.Hostname,
            ["Lightstreamer:MetadataAdapter:RequestReplyPort"] = _container.GetMappedPublicPort(MetadataAdapterPort).ToString()
        };

        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(settings);
        builder.Logging.ClearProviders();
        builder.Services.AddFxRatesProvider(builder.Configuration);

        return builder.Build();
    }

    private static string GetAdapterSetDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Lightstreamer", "FxRatesProvider");
    }

    private void EnsureInitialized()
    {
        if (_container is null)
        {
            throw new InvalidOperationException("The integration-test fixture has not been initialized.");
        }
    }
}
