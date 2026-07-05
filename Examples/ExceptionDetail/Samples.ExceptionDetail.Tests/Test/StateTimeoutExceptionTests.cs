using Samples.ExceptionDetail.Tests.Scenarios;
using Samples.ExceptionDetail.Tests.States;
using Xcepto;
using Xcepto.Config;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.ExceptionDetail.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class StateTimeoutExceptionTests: IStateBuilderIdentity
{
    private XceptoTest _xceptoTest;
    public StateTimeoutExceptionTests(BaseExecutionStrategy executionStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy);
    }


    [Test]
    public void LongTransition_Throws_Total()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(1, 30);
        Assert.ThrowsAsync<TotalTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongTransitionState("long transition state"));
            })
        );
    }
    
    [Test]
    public void LongFailingTransition_Throws_Total()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(1, 30);
        Assert.ThrowsAsync<TotalTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongFailingTransitionState("long failing transition state"));
            })
        );
    }
    
    [Test]
    public void LongTransition_Throws_Test()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(30, 1);
        Assert.ThrowsAsync<TestTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongTransitionState("long transition state"));
            })
        );
    }
    
    [Test]
    public void LongFailingTransition_Throws_Test()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(30, 1);
        Assert.ThrowsAsync<TestTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongFailingTransitionState("long failing transition state"));
            })
        );
    }
    
    [Test]
    public void LongEnter_Throws_Total()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(1, 30);
        Assert.ThrowsAsync<TotalTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongEnterState("long enter state"));
            })
        );
    }
    
    [Test]
    public void LongFailingEnter_Throws_Total()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(1, 30);
        Assert.ThrowsAsync<TotalTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongFailingEnterState("long failing enter state"));
            })
        );
    }
    
    [Test]
    public void LongEnter_Throws_Test()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(30, 1);
        Assert.ThrowsAsync<TestTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongEnterState("long enter state"));
            })
        );
    }
    
    [Test]
    public void LongFailingEnter_Throws_Test()
    {
        var timeoutConfig = TimeoutConfig.FromSeconds(30, 1);
        Assert.ThrowsAsync<TestTimeoutException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), timeoutConfig, builder =>
            {
                builder.AddStep(new LongFailingEnterState("long failing enter state"));
            })
        );
    }
}