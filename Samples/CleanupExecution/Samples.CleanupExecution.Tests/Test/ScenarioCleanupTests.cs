using Samples.CleanupExecution.Tests.Scenario;
using Xcepto;
using Xcepto.Exceptions;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.CleanupExecution.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class ScenarioCleanupTests
{
    private readonly XceptoTest _test;
    public ScenarioCleanupTests(BaseExecutionStrategy executionStrategy)
    {
        _test = new XceptoTest(executionStrategy);
    }

    [Test]
    public async Task CleanupHappens_OnSuccessfulRun()
    {
        TrackableCleanupScenario scenario = new TrackableCleanupScenario();
        await _test.GivenWithStrategies(scenario, _ => { });
        
        Assert.That(scenario.CleanupRan, Is.True);
    }
    
    [Test]
    public void CleanupHappens_OnFailingScenarioInitDo()
    {
        FailingInitDoTrackableCleanupScenario scenario = new FailingInitDoTrackableCleanupScenario();
        
        Assert.That(async () =>
        {
            await _test.GivenWithStrategies(scenario, _ => { });
        }, Throws.InstanceOf<XceptoStageException>());
        
        Assert.That(scenario.CleanupRan, Is.True);
    }
}