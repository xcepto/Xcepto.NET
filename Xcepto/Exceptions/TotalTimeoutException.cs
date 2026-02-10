using System;

namespace Xcepto.Exceptions;

public class TotalTimeoutException : TimeoutException
{
    public TotalTimeoutException(string message, AssertionException innerException) : base(message, innerException)
    {
    }
    public TotalTimeoutException(string message) : base(message)
    {
    }
}