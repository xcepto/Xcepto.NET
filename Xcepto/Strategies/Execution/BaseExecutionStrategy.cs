using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Internal;

namespace Xcepto.Strategies.Execution;

public abstract class BaseExecutionStrategy
{
    internal TestInstance? _testInstance;

    private DateTime _testStartTime;
    protected void StartTest()
    {
        _testStartTime = DateTime.UtcNow;
    }
    protected void CheckTestTimeout()
    {
        FlushLogs();
        if (DateTime.UtcNow >= (_testStartTime + _testInstance.GetTimeout().Test))
        {
            var failingResult = _testInstance.StateMachine?.CurrentXceptoState.MostRecentFailingResult;
            var timeoutMessage = $"Test exceeded TEST timeout: {_testInstance.GetTimeout().Test}";
            if(failingResult is null)
                throw new TestTimeoutException(timeoutMessage);
            throw new TestTimeoutException(timeoutMessage, new AssertionException(failingResult.FailureDescription));
        }
    }
    protected void CheckTimeouts(DateTime deadline)
    {
        FlushLogs();
        if (DateTime.UtcNow >= deadline)
        {
            var failingResult = _testInstance.StateMachine?.CurrentXceptoState.MostRecentFailingResult;
            var timeoutMessage = $"Test exceeded TOTAL timeout: {_testInstance.GetTimeout().Total}";
            if(failingResult is null)
                throw new TotalTimeoutException(timeoutMessage);
            throw new TotalTimeoutException(timeoutMessage, new AssertionException(failingResult.FailureDescription));
        }
    }

    private void FlushLogs()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_testInstance is null || _testInstance.ServiceProvider is null) 
            return;
        var loggingProvider = _testInstance.ServiceProvider.GetRequiredService<ILoggingProvider>();
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
        ExceptionDispatchInfo.Capture(inner).Throw();
    }

    internal void Prime(TestInstance testInstance)
    {
        _testInstance = testInstance;
    }
}