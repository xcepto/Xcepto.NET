using System;

namespace Xcepto.Exceptions;

public class TotalTimeoutException(string message) : TimeoutException(message) { }