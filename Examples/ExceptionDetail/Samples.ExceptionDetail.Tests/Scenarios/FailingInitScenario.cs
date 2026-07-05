using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.ExceptionDetail.Tests.Scenarios;

public class FailingInitScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder)
    {
        throw new Exception();
    }
}