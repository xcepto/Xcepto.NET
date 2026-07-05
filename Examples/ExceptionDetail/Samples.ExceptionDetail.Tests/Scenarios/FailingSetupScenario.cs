using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.ExceptionDetail.Tests.Scenarios;

public class FailingSetupScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder)
    {
        throw new Exception();
    }
}