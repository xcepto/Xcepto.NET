using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;

namespace Xcepto.Strategies.Execution;

public sealed class EnumeratedExecutionStrategy : IPrimeAbleExecutionStrategy
{
    private TestInstance? _testExecution;

    public IEnumerator RunEnumerated()
    {
        if (_testExecution is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasks = _testExecution.GetPropagatedTasks().ToArray();
        var timeout = _testExecution.GetTimeout();
        var deadline = DateTime.UtcNow + timeout;

        // INIT
        var init = _testExecution.InitializeAsync();
        while (!init.IsCompleted)
        {
            yield return null;
            CheckTimeout(deadline);
            CheckPropagatedTasks(propagatedTasks);
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
                CheckPropagatedTasks(propagatedTasks);
            }

            if (stepTask.Result == StepResult.Finished)
                break;

            // a frame passes
            yield return null;

            CheckTimeout(deadline);
            CheckPropagatedTasks(propagatedTasks);
        }

        // CLEANUP
        var cleanup = _testExecution.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            yield return null;
            CheckTimeout(deadline);
            CheckPropagatedTasks(propagatedTasks);
        }
    }

    void IPrimeAbleExecutionStrategy.Prime(TestInstance testInstance)
    {
        _testExecution = testInstance;
    }

    private static void CheckTimeout(DateTime deadline)
    {
        if (DateTime.UtcNow >= deadline)
            throw new TimeoutException("Test exceeded timeout.");
    }

    private static void CheckPropagatedTasks(IEnumerable<Task> propagatedTasks)
    {
        foreach (var task in propagatedTasks)
        {
            if (task.IsFaulted && task.Exception != null)
                ExceptionDispatchInfo.Capture(
                    task.Exception.InnerException ?? task.Exception
                ).Throw();
        }
    }
}