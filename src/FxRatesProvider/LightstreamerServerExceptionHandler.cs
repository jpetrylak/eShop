using Lightstreamer.DotNet.Server;

namespace FxRatesProvider;

internal sealed class LightstreamerServerExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly string _serverRole;
    private readonly Func<bool> _isStopping;
    private int _stopRequested;

    public LightstreamerServerExceptionHandler(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        string serverRole,
        Func<bool> isStopping)
    {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serverRole = serverRole;
        _isStopping = isStopping;
    }

    public bool handleIOException(Exception exception)
    {
        if (_isStopping())
        {
            _logger.LogDebug(exception, "Lightstreamer {ServerRole} transport closed during shutdown.", _serverRole);
            return false;
        }

        _logger.LogError(exception, "Lightstreamer {ServerRole} transport failed.", _serverRole);
        RequestShutdown();
        return false;
    }

    public bool handleException(Exception exception)
    {
        if (_isStopping())
        {
            _logger.LogDebug(exception, "Lightstreamer {ServerRole} reported an exception during shutdown.", _serverRole);
            return false;
        }

        _logger.LogError(exception, "Lightstreamer {ServerRole} reported an unexpected error.", _serverRole);
        RequestShutdown();
        return false;
    }

    private void RequestShutdown()
    {
        if (Interlocked.Exchange(ref _stopRequested, 1) == 1)
        {
            return;
        }

        Environment.ExitCode = 1;
        _applicationLifetime.StopApplication();
    }
}
