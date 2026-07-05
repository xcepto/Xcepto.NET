using Xcepto.Builder;
using Xcepto.Data;

namespace Samples.CleanupExecution.Tests.Scenario;

public class SuccessfulScenario: TrackableCleanupScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();
}