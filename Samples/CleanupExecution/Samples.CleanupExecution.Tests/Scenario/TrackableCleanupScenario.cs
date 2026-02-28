using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.CleanupExecution.Tests.Scenario;

public class TrackableCleanupScenario : XceptoScenario
{
    public bool CleanupRan { get; private set; } = false;
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();

    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder
        .Do(() => CleanupRan = true)
        .Build();
}