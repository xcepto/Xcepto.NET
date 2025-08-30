using Xcepto.Interfaces;

namespace Timeout.Tests.Providers;

public class LoggingProvider: ILoggingProvider
{
    public void LogDebug(string message)
    {
        Console.WriteLine(message);
    }
}