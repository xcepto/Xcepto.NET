using DotNet.Testcontainers.Configurations;
using Xcepto.Interfaces;

namespace Samples.SSR.GUI.Tests.Util;

public class OutputConsumerProvider: IDisposable
{
    private List<IOutputConsumer> _outputConsumers = new();
    private ILoggingProvider _loggingProvider;

    public OutputConsumerProvider(ILoggingProvider loggingProvider)
    {
        _loggingProvider = loggingProvider;
    }

    public void Dispose()
    {
        foreach (var outputConsumer in _outputConsumers)
        {
            outputConsumer.Dispose();
        }
    }

    public IOutputConsumer Create(string prefix, bool verbose = true)
    {
        var loggingOutputConsumer = new LoggingOutputConsumer(_loggingProvider, prefix, verbose);
        _outputConsumers.Add(loggingOutputConsumer);
        return loggingOutputConsumer;
    }
}