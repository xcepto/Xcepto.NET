using System;

namespace Xcepto.Exceptions;

public class AssertionException : Exception
{
    public AssertionException(string message): base(message) { }
}