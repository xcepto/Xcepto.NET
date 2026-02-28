using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;
using Xcepto.Internal;

namespace Xcepto.Strategies.Execution;

public sealed class EnumeratedExecutionStrategy: BaseExecutionStrategy
{
    public IEnumerator RunEnumerated()
    {
        if (testInstance is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasksSupplier = testInstance.GetPropagatedTasksSupplier();
        var timeout = testInstance.GetTimeout();
        var deadline = DateTime.UtcNow + timeout.Total;

        var setup = testInstance.SetupAsync();
        while (!setup.IsCompleted)
        {
            yield return null;
            CheckTimeouts(deadline);
        }
        CheckTimeouts(deadline);
        serviceProvider = setup.GetAwaiter().GetResult();
        
        // INIT
        var init = testInstance.InitializeAsync(serviceProvider);
        while (!init.IsCompleted)
        {
            yield return null;
            CheckTimeouts(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        CheckTimeouts(deadline);
        CheckPropagated(propagatedTasksSupplier);
        init.GetAwaiter().GetResult();

        // EXECUTION LOOP
        StartTest();
        while (true)
        {
            var stepTask = testInstance.StepAsync(serviceProvider);

            while (!stepTask.IsCompleted)
            {
                yield return null;
                CheckTestTimeout();
                CheckTimeouts(deadline);
                CheckPropagated(propagatedTasksSupplier);
            }
            CheckTestTimeout();
            CheckTimeouts(deadline);
            CheckPropagated(propagatedTasksSupplier);

            if (stepTask.GetAwaiter().GetResult() == StepResult.Finished)
                break;

            // a frame passes
            yield return null;
            CheckTestTimeout();
            CheckTimeouts(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }

        // CLEANUP
        var cleanup = testInstance.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            yield return null;
            CheckTimeouts(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        if (cleanup.IsFaulted)
            throw cleanup.Exception?.InnerExceptions.First() ?? new Exception("cleanup task failed without exception");
        CheckTimeouts(deadline);
        CheckPropagated(propagatedTasksSupplier);
    }
}