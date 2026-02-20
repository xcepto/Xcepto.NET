using System;

namespace Xcepto.Exceptions;

public class ScenarioInitException(string message) : XceptoStageException(message);