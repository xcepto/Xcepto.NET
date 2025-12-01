using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;

namespace Xcepto.Strategies.Execution;

public sealed class EnumeratedExecutionStrategy: BaseExecutionStrategy, IPrimeAbleExecutionStrategy
{
    private TestInstance? _testExecution;

    public IEnumerator RunEnumerated()
    {
        if (_testExecution is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasksSupplier = _testExecution.GetPropagatedTasksSupplier();
        var timeout = _testExecution.GetTimeout();
        var deadline = DateTime.UtcNow + timeout;

        // INIT
        var init = _testExecution.InitializeAsync();
        while (!init.IsCompleted)
        {
            yield return null;
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        var serviceProvider = init.Result;

        // EXECUTION LOOP
        while (true)
        {
            var stepTask = _testExecution.StepAsync(serviceProvider);

            while (!stepTask.IsCompleted)
            {
                yield return null;
                CheckTimeout(deadline);
                CheckPropagated(propagatedTasksSupplier);
            }

            if (stepTask.Result == StepResult.Finished)
                break;

            // a frame passes
            yield return null;

            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }

        // CLEANUP
        var cleanup = _testExecution.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            yield return null;
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
    }

    void IPrimeAbleExecutionStrategy.Prime(TestInstance testInstance)
    {
        _testExecution = testInstance;
    }
}