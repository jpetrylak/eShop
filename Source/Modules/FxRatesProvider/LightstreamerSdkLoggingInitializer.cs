using Lightstreamer.DotNet.Server;

namespace FxRatesProvider;

public sealed class LightstreamerSdkLoggingInitializer
{
    public LightstreamerSdkLoggingInitializer(ILoggerFactory loggerFactory)
    {
        Server.SetLoggerProvider(new LightstreamerMicrosoftLoggerProvider(loggerFactory));
    }
}
