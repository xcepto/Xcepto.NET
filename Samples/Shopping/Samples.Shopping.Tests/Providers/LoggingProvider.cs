using Microsoft.Extensions.Logging;
using Xcepto.Interfaces;

namespace Samples.Shopping.Tests.Providers;

public class LoggingProvider: ILoggingProvider, ILogger
{
    public void LogDebug(string message)
    {
        TestContext.Out.WriteLine(message);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if(logLevel == LogLevel.Debug)
            return;
        if(logLevel == LogLevel.Trace)
            return;
        if(logLevel == LogLevel.Information)
            return;
        var formatted = formatter(state, exception);
        LogDebug($"[{logLevel}] {formatted}");
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => new DisposableDummy();
    
    private class DisposableDummy : IDisposable
    {
        public void Dispose() { }
    }
}