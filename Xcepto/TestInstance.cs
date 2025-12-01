using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Data;
using Xcepto.Interfaces;

namespace Xcepto;

internal class TestInstance
{
    private TimeSpan _timeout;
    private DateTime _startTime;
    private BaseScenario _scenario;
    private AcceptanceStateMachine _stateMachine;
    private IEnumerable<XceptoState> _states;
    private IEnumerable<XceptoAdapter> _adapters;
    private Func<IEnumerable<Task>> _propagatedTasksSupplier;

    internal TestInstance(TimeSpan timeout, BaseScenario scenario, TransitionBuilder transitionBuilder)
    {
        _stateMachine = transitionBuilder.Build();
        _propagatedTasksSupplier = () => transitionBuilder.PropagatedTasks;
        _states = transitionBuilder.GetStates();
        _adapters = transitionBuilder.GetAdapters();
        _scenario = scenario;
        _timeout = timeout;
    }

    internal async Task<IServiceProvider> InitializeAsync()
    {
        IServiceProvider serviceProvider = await _scenario.CallSetup();
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug("Setting up acceptance test");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("Added scenario services successfully ✅");
        loggingProvider.LogDebug("");
        
        loggingProvider.LogDebug("Initializing states:");
        loggingProvider.LogDebug($"State initialized: Start (1/{_states.Count()+2})");
        foreach (var (state, counter) in _states.Select((state, counter) => (state, counter+2)))
        {
            await state.Initialize(serviceProvider);
            loggingProvider.LogDebug($"State initialized: {state} ({counter}/{_states.Count()+2})");
        }
        loggingProvider.LogDebug($"State initialized: Final ({_states.Count()+2}/{_states.Count()+2})");
        loggingProvider.LogDebug($"All {_states.Count()+2} states successfully initialized ✅");

        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("Initializing adapters:");
        foreach (var (adapter, counter) in _adapters.Select((adapter, i) => (adapter, i+1)))
        {
            await adapter.CallInitialize(serviceProvider);
            loggingProvider.LogDebug($"Adapter initialized: {adapter} ({counter}/{_adapters.Count()})");
        }
        
        loggingProvider.LogDebug($"All {_adapters.Count()} adapters successfully initialized ✅");
        loggingProvider.LogDebug("");
        await _scenario.CallInitialize(serviceProvider);
        loggingProvider.LogDebug("Initialized scenario successfully ✅");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("Setup complete, starting test now:");
        loggingProvider.LogDebug("---------------------------------");
        
        _startTime = DateTime.Now;
        await _stateMachine.Start(serviceProvider);
        return serviceProvider;
    }

    internal async Task<StepResult> StepAsync(IServiceProvider serviceProvider)
    {
        if (_stateMachine.CurrentXceptoState.Equals(_stateMachine.FinalXceptoState))
            return StepResult.Finished;

        if (DateTime.Now - _startTime >= _timeout)
            return StepResult.Canceled;

        await _stateMachine.TryTransition(serviceProvider);
        return StepResult.Continue;
    }

    internal async Task CleanupAsync(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug("---------------------------------");
        loggingProvider.LogDebug("Test completed ✅");
        loggingProvider.LogDebug("");
        
        loggingProvider.LogDebug("Cleaning up:");
        foreach (var (adapter, counter) in _adapters.Select((adapter, i) => (adapter, i + 1)))
        {
            await adapter.CallCleanup(serviceProvider);
            loggingProvider.LogDebug($"Adapter cleanup: {adapter} ({counter}/{_adapters.Count()})");
        }
        loggingProvider.LogDebug($"All {_adapters.Count()} adapters were successfully cleaned up ✅");

        await _scenario.CallCleanup(serviceProvider);
        
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("=================================");
        loggingProvider.LogDebug("TEST DONE");
        loggingProvider.LogDebug("=================================");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("");
    }

    internal TimeSpan GetTimeout() => _timeout;

    public Func<IEnumerable<Task>> GetPropagatedTasksSupplier() => _propagatedTasksSupplier;
}
