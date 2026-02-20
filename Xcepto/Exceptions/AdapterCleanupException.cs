using System;

namespace Xcepto.Exceptions;

public class AdapterCleanupException(string message) : XceptoStageException(message);