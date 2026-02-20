using System;

namespace Xcepto.Exceptions;

public class ScenarioInitException(string message, Exception inner) : XceptoStageException(message, inner);