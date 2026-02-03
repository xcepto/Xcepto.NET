using System;

namespace Xcepto.Exceptions;

public class TestTimeoutException(string message) : TimeoutException(message) { }