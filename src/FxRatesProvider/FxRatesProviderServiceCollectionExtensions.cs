namespace FxRatesProvider;

public static class FxRatesProviderServiceCollectionExtensions
{
    public static IServiceCollection AddFxRatesProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<FxRatesProviderOptions>()
            .Bind(configuration);

        services.AddSingleton<LightstreamerSdkLoggingInitializer>();
        services.AddSingleton<FxRatesDataAdapter>();
        services.AddSingleton<FxRatesMetadataProvider>();
        services.AddHostedService<FxRatesProviderWorker>();

        return services;
    }
}
