using System;
using Xcepto.Interfaces;

namespace Xcepto.Provider;

public class XceptoBasicLoggingProvider: ILoggingProvider
{
    public void LogDebug(string message)
    {
        Console.WriteLine(message);
    }
}