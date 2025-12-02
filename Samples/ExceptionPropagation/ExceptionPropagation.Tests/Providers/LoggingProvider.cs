using Xcepto.Interfaces;

namespace ExceptionPropagation.Tests.Providers;

public class LoggingProvider: ILoggingProvider
{
    public void LogDebug(string message)
    {
        Console.WriteLine(message);
    }
}