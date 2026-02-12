using System;

namespace Xcepto.Exceptions;

public class PromiseException: Exception
{
    public PromiseException(string message) : base(message)
    {
        
    }
}