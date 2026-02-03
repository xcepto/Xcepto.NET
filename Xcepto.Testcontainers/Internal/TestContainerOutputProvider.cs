using System.Collections.Generic;
using DotNet.Testcontainers.Configurations;
using Microsoft.Extensions.Logging;
using Xcepto.Interfaces;
using Xcepto.Testcontainers.Interfaces;

namespace Xcepto.Testcontainers.Internal;

internal sealed class TestContainerOutputProvider: ITestContainerSupport
{
    private List<IOutputConsumer> _outputConsumers = new();
    private List<ILoggerFactory> _loggerFactories = new();
    private ILoggingProvider _loggingProvider;

    public TestContainerOutputProvider(ILoggingProvider loggingProvider)
    {
        _loggingProvider = loggingProvider;
    }

    public void Dispose()
    {
        foreach (var outputConsumer in _outputConsumers)
        {
            outputConsumer.Dispose();
        }

        foreach (var loggerFactory in _loggerFactories)
        {
            loggerFactory.Dispose();
        }
    }

    public IOutputConsumer CreateOutputConsumer(string prefix, bool verbose = true)
    {
        var loggingOutputConsumer = new LoggingOutputConsumer(_loggingProvider, prefix, verbose);
        _outputConsumers.Add(loggingOutputConsumer);
        return loggingOutputConsumer;
    }

    public ILogger CreateLogger(string prefix, bool verbose = true)
    {
        var loggerProvider = new TestcontainersLoggerProvider(_loggingProvider);
        var loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder
                .SetMinimumLevel(LogLevel.Warning)
                .AddProvider(loggerProvider);
        });
    
        var baseLogger = loggerFactory.CreateLogger(prefix);
        if(!verbose)
            baseLogger = new TestContainersLogFilter(baseLogger);

        return baseLogger;
    }
}