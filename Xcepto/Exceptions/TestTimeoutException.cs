using System;

namespace Xcepto.Exceptions;

public class TestTimeoutException : TimeoutException
{
    public TestTimeoutException(string message, AssertionException innerException) : base(message, innerException)
    {
    }
    
    public TestTimeoutException(string message) : base(message)
    {
    }
}