using System.Collections.Generic;
using Xcepto.Strategies.Execution;
using Xcepto.Strategies.Isolation;
using Xcepto.Strategies.Scheduling;

namespace Xcepto.Strategies;

public static class StrategyCombinations
{
    public static IEnumerable<object[]> AllCombinations()
    {
        var executionStrategies = new IExecutionStrategy[] { new AsyncExecutionStrategy(), new EnumeratedExecutionStrategy() };
        var schedulingStrategies = new ISchedulingStrategy[] { new ParallelSchedulingStrategy(), new SequentialSchedulingStrategy() };
        var isolationStrategies  = new IIsolationStrategy[] { new NoIsolationStrategy(), new CompartmentalizedIsolationStrategy() };

        foreach (var e in executionStrategies)
        foreach (var s in schedulingStrategies)
        foreach (var i in isolationStrategies)
            yield return new object[] { e, s, i };
    }

}