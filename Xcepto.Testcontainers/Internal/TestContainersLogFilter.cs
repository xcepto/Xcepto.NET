using System;
using Microsoft.Extensions.Logging;

namespace Xcepto.Testcontainers.Internal;

internal sealed class TestContainersLogFilter : ILogger
{
    private readonly ILogger _inner;

    public TestContainersLogFilter(ILogger inner)
        => _inner = inner;

    public IDisposable BeginScope<TState>(TState state) => _inner.BeginScope(state);
    public bool IsEnabled(LogLevel logLevel) => _inner.IsEnabled(logLevel);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        if (message.Contains("regex cache"))
            return;

        _inner.Log(logLevel, eventId, state, exception, formatter);
    }
}