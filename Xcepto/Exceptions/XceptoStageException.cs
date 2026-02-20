using System;

namespace Xcepto.Exceptions;

public abstract class XceptoStageException(string message): Exception(message);