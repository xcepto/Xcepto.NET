using System;

namespace Xcepto.Exceptions;

public class AdapterInitException(string message) : XceptoStageException(message);