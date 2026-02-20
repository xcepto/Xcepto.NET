using System;

namespace Xcepto.Exceptions;

public class TotalTimeoutException : XceptoTimeoutException
{
    public TotalTimeoutException(string message) : base(message)
    {
    }
}