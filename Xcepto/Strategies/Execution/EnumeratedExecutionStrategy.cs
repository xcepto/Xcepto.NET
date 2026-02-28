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
    private DateTime _deadline;
    private Func<IEnumerable<Task>> _propagatedTasksSupplier;

    public IEnumerator RunEnumerated()
    {
        if (testInstance is null)
            throw new Exception("Execution strategy not primed yet");

        _propagatedTasksSupplier = testInstance.GetPropagatedTasksSupplier();
        var timeout = testInstance.GetTimeout();
        _deadline = DateTime.UtcNow + timeout.Total;

        var setup = testInstance.SetupAsync();
        while (!setup.IsCompleted)
        {
            yield return null;
            CheckTimeouts(_deadline);
        }
        CheckTimeouts(_deadline);
        serviceProvider = setup.GetAwaiter().GetResult();

        try
        {
            // INIT
            var init = testInstance.InitializeAsync(serviceProvider);
            while (!init.IsCompleted)
            {
                yield return null;
                CheckTimeouts(_deadline);
                CheckPropagated(_propagatedTasksSupplier);
            }
            CheckTimeouts(_deadline);
            CheckPropagated(_propagatedTasksSupplier);
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
                    CheckTimeouts(_deadline);
                    CheckPropagated(_propagatedTasksSupplier);
                }
                CheckTestTimeout();
                CheckTimeouts(_deadline);
                CheckPropagated(_propagatedTasksSupplier);

                if (stepTask.GetAwaiter().GetResult() == StepResult.Finished)
                    break;

                // a frame passes
                yield return null;
                CheckTestTimeout();
                CheckTimeouts(_deadline);
                CheckPropagated(_propagatedTasksSupplier);
            }
        }
        finally
        {
            var enumerator = Cleanup();
            while (enumerator.MoveNext())
            {
                CheckTimeouts(_deadline);
                CheckPropagated(_propagatedTasksSupplier);
            }
            CheckTimeouts(_deadline);
            CheckPropagated(_propagatedTasksSupplier);
        }
    }

    private IEnumerator Cleanup()
    {
        // CLEANUP
        var cleanup = testInstance.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            yield return null;
            CheckTimeouts(_deadline);
            CheckPropagated(_propagatedTasksSupplier);
        }
        if (cleanup.IsFaulted)
            throw cleanup.Exception?.InnerExceptions.First() ?? new Exception("cleanup task failed without exception");
        CheckTimeouts(_deadline);
        CheckPropagated(_propagatedTasksSupplier);
    }
}