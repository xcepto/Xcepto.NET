using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Samples.CleanupExecution.Tests.Scenario;

public abstract class TrackableCleanupScenario : XceptoScenario
{
    public bool CleanupRan { get; private set; } = false;
    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder
        .Do(() => CleanupRan = true)
        .Build();
}