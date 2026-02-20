using System;

namespace Xcepto.Exceptions;

public class ScenarioCleanupException(string message) : XceptoStageException(message);