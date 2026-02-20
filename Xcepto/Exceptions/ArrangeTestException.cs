using System;

namespace Xcepto.Exceptions;

public class ArrangeTestException(string message) : XceptoStageException(message);