using System.Collections.Concurrent;
using Xcepto.Interfaces;

namespace LoggerDisposalTests.Provider;

public class MockedLoggingProvider: ILoggingProvider
{
    private ConcurrentQueue<string?> _logs = new();
    private List<string> _flushedLogs = new();
    public void Dispose() => Flush();

    public void LogDebug(string message)
    {
        _logs.Enqueue(message);
    }

    public void Flush()
    {
        while (!_logs.IsEmpty)
        {
            if (_logs.TryDequeue(out string? message) && message is not null)
            {
                Console.WriteLine(message);
                _flushedLogs.Add(message);
            }
        }
    }

    public bool Flushed(string message)
    {
        return _flushedLogs.Any(x => x.Equals(message));
    }
}