using Microsoft.Extensions.Logging;
using Xcepto.Interfaces;

namespace Xcepto.Testcontainers.Internal;

internal sealed class TestcontainersLoggerProvider: ILoggerProvider
{
    private ILoggingProvider _loggingProvider;

    public TestcontainersLoggerProvider(ILoggingProvider loggingProvider)
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