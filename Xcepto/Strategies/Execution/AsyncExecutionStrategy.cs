using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;

namespace Xcepto.Strategies.Execution;

public sealed class AsyncExecutionStrategy : IExecutionStrategy
{
    private TestInstance? _testInstance;

    public async Task RunAsync()
    {
        if (_testInstance is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasks = _testInstance.GetPropagatedTasks().ToArray();
        var timeout = _testInstance.GetTimeout();
        var deadline = DateTime.UtcNow + timeout;

        // INIT
        var init = _testInstance.InitializeAsync();
        while (!init.IsCompleted)
        {
            await Task.Yield();
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasks);
        }
        var serviceProvider = init.Result;

        // EXECUTION LOOP
        while (true)
        {
            var stepTask = _testInstance.StepAsync(serviceProvider);

            while (!stepTask.IsCompleted)
            {
                await Task.Yield();
                CheckTimeout(deadline);
                CheckPropagated(propagatedTasks);
            }

            if (stepTask.Result == StepResult.Finished)
                break;

            await Task.Yield();
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasks);
        }

        // CLEANUP
        var cleanup = _testInstance.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            await Task.Yield();          
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasks);
        }
    }

    internal void Prime(TestInstance testInstance)
    {
        _testInstance = testInstance;
    }

    private static void CheckTimeout(DateTime deadline)
    {
        if (DateTime.UtcNow >= deadline)
            throw new TimeoutException("Test exceeded timeout.");
    }

    private static void CheckPropagated(IEnumerable<Task> propagatedTasks)
    {
        foreach (var tasks in propagatedTasks)
        {
            if (tasks.IsFaulted && tasks.Exception != null)
            {
                var ex = tasks.Exception.InnerException ?? tasks.Exception;
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }
    }
}
