using System;

namespace Xcepto.Exceptions;

public class TotalTimeoutException : XceptoStageException
{
    public TotalTimeoutException(string message) : base(message)
    {
    }
}