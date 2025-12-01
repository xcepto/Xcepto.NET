using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Xcepto.Strategies.Execution;

public abstract class BaseExecutionStrategy
{
    protected static void CheckTimeout(DateTime deadline)
    {
        if (DateTime.UtcNow >= deadline)
            throw new TimeoutException("Test exceeded timeout.");
    }

    protected static void CheckPropagated(Func<IEnumerable<Task>> propagatedTasksSupplier)
    {
        var firstFaulted = propagatedTasksSupplier()
            .FirstOrDefault(t => t.IsFaulted && t.Exception is not null);

        if (firstFaulted is null)
            return;

        // Unwrap AggregateException EXACTLY like before
        var ex = firstFaulted.Exception;
        var inner = ex.InnerException ?? ex;

        ExceptionDispatchInfo.Capture(inner).Throw();
    }
}