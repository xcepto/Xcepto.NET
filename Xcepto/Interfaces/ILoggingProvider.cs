using System;

namespace Xcepto.Interfaces
{
    public interface ILoggingProvider: IDisposable
    {
        void LogDebug(string message);
        void Flush();
    }
}