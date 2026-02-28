using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;
using Xcepto.Internal;

namespace Xcepto.Strategies.Execution;

public sealed class AsyncExecutionStrategy : BaseExecutionStrategy
{
    public async Task RunAsync()
    {
        if (testInstance is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasksSupplier = testInstance.GetPropagatedTasksSupplier();
        var timeout = testInstance.GetTimeout();
        var totalDeadline = DateTime.UtcNow + timeout.Total;

        var setup = testInstance.SetupAsync();
        while (!setup.IsCompleted)
        {
            await Task.Yield();
            CheckTimeouts(totalDeadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        CheckTimeouts(totalDeadline);
        CheckPropagated(propagatedTasksSupplier);
        serviceProvider = setup.GetAwaiter().GetResult();

        // INIT
        var init = testInstance.InitializeAsync(serviceProvider);
        while (!init.IsCompleted)
        {
            await Task.Yield();
            CheckTimeouts(totalDeadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        CheckTimeouts(totalDeadline);
        CheckPropagated(propagatedTasksSupplier);
        init.GetAwaiter().GetResult();

        // EXECUTION LOOP
        StartTest();
        while (true)
        {
            var stepTask = testInstance.StepAsync(serviceProvider);

            while (!stepTask.IsCompleted)
            {
                await Task.Yield();
                CheckTestTimeout();
                CheckTimeouts(totalDeadline);
                CheckPropagated(propagatedTasksSupplier);
            }
            CheckTestTimeout();
            CheckTimeouts(totalDeadline);
            CheckPropagated(propagatedTasksSupplier);
            if (stepTask.GetAwaiter().GetResult() == StepResult.Finished)
                break;

            await Task.Yield();
            CheckTestTimeout();
            CheckTimeouts(totalDeadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        
        // CLEANUP
        var cleanup = testInstance.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            await Task.Yield();          
            CheckTimeouts(totalDeadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        if (cleanup.IsFaulted)
            throw cleanup.Exception?.InnerExceptions.First() ?? new Exception("cleanup task failed without exception");
        CheckTimeouts(totalDeadline);
        CheckPropagated(propagatedTasksSupplier);
    }
}
