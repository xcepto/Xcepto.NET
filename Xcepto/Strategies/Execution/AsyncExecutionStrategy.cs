using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xcepto.Data;
using Xcepto.Internal;

namespace Xcepto.Strategies.Execution;

public sealed class AsyncExecutionStrategy : BaseExecutionStrategy, IPrimeAbleExecutionStrategy
{
    private TestInstance? _testInstance;

    public async Task RunAsync()
    {
        if (_testInstance is null)
            throw new Exception("Execution strategy not primed yet");

        var propagatedTasksSupplier = _testInstance.GetPropagatedTasksSupplier();
        var timeout = _testInstance.GetTimeout();
        var deadline = DateTime.UtcNow + timeout;

        // INIT
        var init = _testInstance.InitializeAsync();
        while (!init.IsCompleted)
        {
            await Task.Yield();
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        CheckTimeout(deadline);
        CheckPropagated(propagatedTasksSupplier);
        var serviceProvider = init.GetAwaiter().GetResult();;

        // EXECUTION LOOP
        while (true)
        {
            var stepTask = _testInstance.StepAsync(serviceProvider);

            while (!stepTask.IsCompleted)
            {
                await Task.Yield();
                CheckTimeout(deadline);
                CheckPropagated(propagatedTasksSupplier);
            }
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
            if (stepTask.GetAwaiter().GetResult() == StepResult.Finished)
                break;

            await Task.Yield();
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }

        // CLEANUP
        var cleanup = _testInstance.CleanupAsync(serviceProvider);
        while (!cleanup.IsCompleted)
        {
            await Task.Yield();          
            CheckTimeout(deadline);
            CheckPropagated(propagatedTasksSupplier);
        }
        CheckTimeout(deadline);
        CheckPropagated(propagatedTasksSupplier);
    }

    void IPrimeAbleExecutionStrategy.Prime(TestInstance testInstance)
    {
        _testInstance = testInstance;
    }
}
