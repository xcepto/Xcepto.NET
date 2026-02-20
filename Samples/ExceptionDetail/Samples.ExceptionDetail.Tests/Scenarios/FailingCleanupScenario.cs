using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.ExceptionDetail.Tests.Scenarios;

public class FailingCleanupScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();

    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder)
    {
        throw new Exception();
    }
}