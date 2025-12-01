using System;
using System.Collections.Generic;
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
        foreach (var task in propagatedTasksSupplier())
        {
            if (task.IsFaulted && task.Exception != null)
                ExceptionDispatchInfo.Capture(
                    task.Exception.InnerException ?? task.Exception
                ).Throw();
        }
    }
}