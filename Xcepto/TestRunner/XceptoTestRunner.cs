using System;
using Xcepto.Builder;
using Xcepto.Internal;
using Xcepto.Scenarios;
using Xcepto.Strategies.Execution;

namespace Xcepto.TestRunner;

public class XceptoTestRunner
{
    private IPrimeAbleExecutionStrategy _executionStrategy;

    public XceptoTestRunner(IExecutionStrategy executionStrategy)
    {
        if (executionStrategy is not IPrimeAbleExecutionStrategy primeAbleExecutionStrategy)
            throw new ArgumentException("Unofficial execution strategy");
        _executionStrategy = primeAbleExecutionStrategy;
    }

    public void Given(XceptoScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
    {
        TransitionBuilder transitionBuilder = new TransitionBuilder();
        scenario.AssignBuilder(transitionBuilder);
        builder(transitionBuilder);

        TestInstance testInstance = new TestInstance(timeout, scenario, transitionBuilder);
        _executionStrategy.Prime(testInstance);
    }
}