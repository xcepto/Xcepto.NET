using System;

namespace Xcepto.Exceptions;

public class ScenarioSetupException(string message, Exception inner) : XceptoStageException(message, inner);