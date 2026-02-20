using System;

namespace Xcepto.Exceptions;

public class TestTimeoutException : XceptoStageException
{
    public TestTimeoutException(string message) : base(message)
    {
    }
}