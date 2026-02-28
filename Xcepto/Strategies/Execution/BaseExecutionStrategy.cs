using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Util;

namespace Xcepto.Strategies.Execution;

public abstract class BaseExecutionStrategy
{
    internal TestInstance? testInstance;
    internal IServiceProvider? serviceProvider;

    private DateTime _testStartTime;
    protected void StartTest()
    {
        _testStartTime = DateTime.UtcNow;
    }
    protected void CheckTestTimeout()
    {
        FlushLogs();
        if (DateTime.UtcNow >= (_testStartTime + testInstance.GetTimeout().Test))
        {
            var failingResult = testInstance.StateMachine?.CurrentXceptoState.MostRecentFailingResult;
            string currentState = testInstance?.StateMachine?.CurrentXceptoState.Name ?? "";
            var timeoutMessage = $"Test exceeded TEST timeout: {testInstance.GetTimeout().Test} during [{currentState}]";
            if (serviceProvider is not null)
            {
                var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
                loggingProvider.LogDebug(timeoutMessage);
                FlushLogs();
            }
            if(failingResult is null)
                throw new TestTimeoutException(timeoutMessage);
            throw new TestTimeoutException(timeoutMessage).Promote(new AssertionException(failingResult.FailureDescription));
        }
    }
    protected void CheckTimeouts(DateTime deadline)
    {
        FlushLogs();
        if (DateTime.UtcNow >= deadline)
        {
            var failingResult = testInstance.StateMachine?.CurrentXceptoState.MostRecentFailingResult;
            var timeoutMessage = $"Test exceeded TOTAL timeout: {testInstance.GetTimeout().Total}";
            if (serviceProvider is not null)
            {
                var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
                loggingProvider.LogDebug(timeoutMessage);
                FlushLogs();
            }
            if(failingResult is null)
                throw new TotalTimeoutException(timeoutMessage);
            throw new TotalTimeoutException(timeoutMessage).Promote(new AssertionException(failingResult.FailureDescription));
        }
    }

    private void FlushLogs()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (testInstance is null || serviceProvider is null) 
            return;
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.Flush();
    }

    protected void CheckPropagated(Func<IEnumerable<Task>> propagatedTasksSupplier)
    {
        FlushLogs();
        var tasks = propagatedTasksSupplier();
        var firstFaulted = tasks
            .FirstOrDefault(t => t.IsFaulted && t.Exception is not null);

        if (firstFaulted is null)
            return;
        
        // Unwrap AggregateException EXACTLY like before
        var ex = firstFaulted.Exception;
        var inner = ex.InnerException ?? ex;
        if (serviceProvider is not null)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
            loggingProvider.LogDebug($"Propagated task failed: {inner}");
            FlushLogs();
        }
        ExceptionDispatchInfo.Capture(inner).Throw();
    }

    internal void Prime(TestInstance testInstance)
    {
        this.testInstance = testInstance;
    }
}