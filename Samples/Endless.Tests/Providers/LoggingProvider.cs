using Xcepto.Interfaces;

namespace Endless.Tests.Providers;

public class LoggingProvider: ILoggingProvider
{
    public void LogDebug(string message)
    {
        Console.WriteLine(message);
    }
}