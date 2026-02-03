using System;
using Microsoft.Extensions.Logging;
using Xcepto.Interfaces;

namespace Xcepto.Testcontainers.Internal;

internal sealed class SimpleLogger : ILogger
{
    private readonly ILoggingProvider _provider;

    public SimpleLogger(ILoggingProvider provider)
    {
        _provider = provider;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _provider.LogDebug(formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel) => true;
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}
