using Microsoft.Extensions.Logging;
using Xcepto.Interfaces;

namespace Samples.SSR.GUI.Tests.Util;

public class LoggerProvider: ILoggerProvider
{
    private ILoggingProvider _loggingProvider;

    public LoggerProvider(ILoggingProvider loggingProvider)
    {
        _loggingProvider = loggingProvider;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SimpleLogger(_loggingProvider);
    }

    public void Dispose()
    {
        _loggingProvider.Dispose();
    }
}