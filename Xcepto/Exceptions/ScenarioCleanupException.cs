using System;

namespace Xcepto.Exceptions;

public class ScenarioCleanupException(string message, Exception inner) : XceptoStageException(message, inner);