using Samples.ExceptionDetail.Tests.Scenarios;
using Samples.ExceptionDetail.Tests.States;
using Xcepto;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Strategies;
using Xcepto.Strategies.Execution;

namespace Samples.ExceptionDetail.Tests.Test;

[TestFixtureSource(typeof(StrategyCombinations), nameof(StrategyCombinations.AllCombinations))]
public class StateExceptionTests: IStateBuilderIdentity
{
    private XceptoTest _xceptoTest;
    public StateExceptionTests(BaseExecutionStrategy executionStrategy)
    {
        _xceptoTest = new XceptoTest(executionStrategy);
    }


    [Test]
    public void Direct_FailingStateSetup_Throws()
    {
        Assert.ThrowsAsync<ArrangeTestException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddStep(new FailingSetupState("test construction state"));
            })
        );
    }
    
    [Test]
    public void Future_FailingStateSetup_Throws()
    {
        Assert.ThrowsAsync<ArrangeTestException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddFutureStep(() => new FailingSetupState("test construction state"), this);
            })
        );
    }
    
    [Test]
    public void Direct_FailingStateInit_Throws()
    {
        Assert.ThrowsAsync<StateInitException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddStep(new FailingInitState("test init state"));
            })
        );
    }
    
    [Test]
    public void Future_FailingStateInit_Throws()
    {
        Assert.ThrowsAsync<StateInitException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddFutureStep(() => new FailingInitState("test init state"), this);
            })
        );
    }
    
    
    [Test]
    public void Direct_FailingStateEnter_Throws()
    {
        Assert.ThrowsAsync<StateEnterException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddStep(new FailingEnterState("test enter state"));
            })
        );
    }
    
        
    [Test]
    public void Future_FailingStateEnter_Throws()
    {
        Assert.ThrowsAsync<StateEnterException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddFutureStep(() => new FailingEnterState("test enter state"), this);
            })
        );
    }

    [Test]
    public void Direct_FailingStateTransition_Throws()
    {
        Assert.ThrowsAsync<StateTransitionException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddStep(new FailingTransitionState("test transition state"));
            })
        );
    }
    
    [Test]
    public void Future_FailingStateTransition_Throws()
    {
        Assert.ThrowsAsync<StateTransitionException>(async () => await _xceptoTest.GivenWithStrategies(new CleanScenario(), builder =>
            {
                builder.AddFutureStep(() => new FailingTransitionState("test transition state"), this);
            })
        );
    }
}