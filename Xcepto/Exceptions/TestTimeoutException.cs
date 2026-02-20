using System;

namespace Xcepto.Exceptions;

public class TestTimeoutException : XceptoTimeoutException
{
    public TestTimeoutException(string message) : base(message)
    {
    }
}