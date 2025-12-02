using System.Collections.Generic;
using Xcepto.Strategies.Execution;

namespace Xcepto.Strategies;

public static class StrategyCombinations
{
    public static IEnumerable<object[]> AllCombinations()
    {
        var executionStrategies = new IExecutionStrategy[] { new AsyncExecutionStrategy(), new EnumeratedExecutionStrategy() };

        foreach (var e in executionStrategies)
            yield return new object[] { e };
    }

}