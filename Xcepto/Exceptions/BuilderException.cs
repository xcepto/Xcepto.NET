using System;

namespace Xcepto.Exceptions;

public class BuilderException: Exception
{
    public BuilderException(string message): base(message)
    {
    }
}