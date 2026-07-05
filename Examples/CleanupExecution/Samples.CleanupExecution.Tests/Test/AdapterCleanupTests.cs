using Samples.CleanupExecution.Tests.Adapters;
using Samples.CleanupExecution.Tests.Scenario;
using Xcepto;
using Xcepto.Exceptions;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.CleanupExecution.Tests.Test;


[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class AdapterCleanupTests
{
    private readonly XceptoTest _test;
    public AdapterCleanupTests(BaseExecutionStrategy executionStrategy)
    {
        _test = new XceptoTest(executionStrategy);
    }


    [Test]
    public async Task SuccessfulAdapter_CleanedUp()
    {
        var adapter = new SuccessfulAdapter();
        await _test.GivenWithStrategies(new SuccessfulScenario(), builder =>
        {
            builder.RegisterAdapter(adapter);
        });
        
        Assert.That(adapter.CleanedUp, Is.True);
    }
    
    [Test]
    public void FailingInitAdapter_CleanedUp()
    {
        var adapter = new FailingInitAdapter();
        Assert.That(async () =>
        {
            await _test.GivenWithStrategies(new SuccessfulScenario(), builder =>
            {
                builder.RegisterAdapter(adapter);
            });
        }, Throws.InstanceOf<AdapterInitException>());
        
        Assert.That(adapter.CleanedUp, Is.True);
    }
}