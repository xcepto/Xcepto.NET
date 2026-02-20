using System;

namespace Xcepto.Exceptions;

public class ScenarioSetupException(string message) : XceptoStageException(message);