using System;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Xcepto;

public class XceptoTestRunner
{
    private IPrimeAbleExecutionStrategy _executionStrategy;
    private ISchedulingStrategy _schedulingStrategy;
    private IIsolationStrategy _isolationStrategy;

    public XceptoTestRunner(IExecutionStrategy executionStrategy, ISchedulingStrategy schedulingStrategy, IIsolationStrategy isolationStrategy)
    {
        if (executionStrategy is not IPrimeAbleExecutionStrategy primeAbleExecutionStrategy)
            throw new ArgumentException("Unofficial execution strategy");
        _executionStrategy = primeAbleExecutionStrategy;
        _schedulingStrategy = schedulingStrategy;
        _isolationStrategy = isolationStrategy;
    }

    public void Given(BaseScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
    {
        TransitionBuilder transitionBuilder = new TransitionBuilder();
        scenario.AssignBuilder(transitionBuilder);
        builder(transitionBuilder);

        TestInstance testInstance = new TestInstance(timeout, scenario, transitionBuilder);
        _executionStrategy.Prime(testInstance);
    }
}