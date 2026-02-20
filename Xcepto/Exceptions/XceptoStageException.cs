using System;

namespace Xcepto.Exceptions;

public class XceptoStageException(string message, Exception inner): Exception(message, inner);