using System;
using Xcepto.Builder;
using Xcepto.Internal;
using Xcepto.Scenarios;
using Xcepto.Strategies.Execution;

namespace Xcepto.TestRunner;

public class XceptoTestRunner
{
    private BaseExecutionStrategy _executionStrategy;

    public XceptoTestRunner(BaseExecutionStrategy executionStrategy)
    {
        _executionStrategy = executionStrategy;
    }

    public void Given(XceptoScenario scenario, TimeSpan timeout, Action<TransitionBuilder> builder)
    {
        TransitionBuilder transitionBuilder = new TransitionBuilder(builder);
        scenario.AssignBuilder(transitionBuilder);

        TestInstance testInstance = new TestInstance(timeout, scenario, transitionBuilder);
        _executionStrategy.Prime(testInstance);
    }
}