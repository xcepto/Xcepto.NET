using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.CleanupExecution.Tests.Scenario;

public class FailingInitFireScenario : TrackableCleanupScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder
        .FireAndForget(_ => throw new Exception())
        .Build();
}