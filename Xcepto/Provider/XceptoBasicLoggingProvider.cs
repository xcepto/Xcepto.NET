using System;
using System.Collections.Concurrent;
using Xcepto.Interfaces;

namespace Xcepto.Provider;

public class XceptoBasicLoggingProvider: ILoggingProvider
{
    private ConcurrentQueue<string> _logMessages = new();
    public void LogDebug(string message)
    {
        _logMessages.Enqueue(message);
    }

    public void Flush()
    {
        while (!_logMessages.IsEmpty)
        {
            if (!_logMessages.TryDequeue(out string message))
                throw new InvalidOperationException("Log messages are not consumed correctly");
            Console.WriteLine(message);
        }
    }
}