namespace FxRatesProvider;

internal sealed class LightstreamerMicrosoftLoggerProvider : Lightstreamer.DotNet.Server.Log.ILoggerProvider
{
    private readonly ILoggerFactory _loggerFactory;

    public LightstreamerMicrosoftLoggerProvider(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public Lightstreamer.DotNet.Server.Log.ILogger GetLogger(string category)
    {
        return new LightstreamerMicrosoftLogger(_loggerFactory.CreateLogger(category));
    }

    private sealed class LightstreamerMicrosoftLogger : Lightstreamer.DotNet.Server.Log.ILogger
    {
        private readonly ILogger _logger;

        public LightstreamerMicrosoftLogger(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsDebugEnabled => _logger.IsEnabled(LogLevel.Debug);
        public bool IsInfoEnabled => _logger.IsEnabled(LogLevel.Information);
        public bool IsWarnEnabled => _logger.IsEnabled(LogLevel.Warning);
        public bool IsErrorEnabled => _logger.IsEnabled(LogLevel.Error);
        public bool IsFatalEnabled => _logger.IsEnabled(LogLevel.Critical);

        public void Error(string line)
        {
            _logger.LogError("{Message}", line);
        }

        public void Error(string line, Exception exception)
        {
            _logger.LogError(exception, "{Message}", line);
        }

        public void Warn(string line)
        {
            _logger.LogWarning("{Message}", line);
        }

        public void Warn(string line, Exception exception)
        {
            _logger.LogWarning(exception, "{Message}", line);
        }

        public void Info(string line)
        {
            _logger.LogInformation("{Message}", line);
        }

        public void Info(string line, Exception exception)
        {
            _logger.LogInformation(exception, "{Message}", line);
        }

        public void Debug(string line)
        {
            _logger.LogDebug("{Message}", line);
        }

        public void Debug(string line, Exception exception)
        {
            _logger.LogDebug(exception, "{Message}", line);
        }

        public void Fatal(string line)
        {
            _logger.LogCritical("{Message}", line);
        }

        public void Fatal(string line, Exception exception)
        {
            _logger.LogCritical(exception, "{Message}", line);
        }
    }
}
