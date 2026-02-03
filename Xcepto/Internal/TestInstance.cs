using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Adapters;
using Xcepto.Builder;
using Xcepto.Config;
using Xcepto.Interfaces;
using Xcepto.Scenarios;
using Xcepto.States;

namespace Xcepto.Internal;

internal class TestInstance
{
    private TimeoutConfig _timeout;
    private DateTime _startTime;
    private XceptoScenario _scenario;
    private TransitionBuilder _transitionBuilder;

    private AcceptanceStateMachine? _stateMachine;
    private IEnumerable<XceptoState>? _states;
    private IEnumerable<XceptoAdapter>? _adapters;
    private Func<IEnumerable<Task>>? _propagatedTasksSupplier;
    public IServiceProvider ServiceProvider { get; private set; }

    internal TestInstance(TimeoutConfig timeout, XceptoScenario scenario, TransitionBuilder transitionBuilder)
    {
        _transitionBuilder = transitionBuilder;
        _propagatedTasksSupplier = () => transitionBuilder.PropagatedTasks;
        _scenario = scenario;
        _timeout = timeout;
    }

    internal async Task<IServiceProvider> InitializeAsync()
    {
        ServiceProvider = await _scenario.CallSetup();
        var loggingProvider = ServiceProvider.GetRequiredService<ILoggingProvider>();
        loggingProvider.LogDebug("Setting up acceptance test");
        
        loggingProvider.LogDebug("");
        await _scenario.CallInitialize();
        loggingProvider.LogDebug("Initialized scenario successfully ✅");
        loggingProvider.LogDebug("");
        
        _stateMachine = _transitionBuilder.Build();
        _states = _transitionBuilder.GetStates().ToArray();
        _adapters = _transitionBuilder.GetAdapters().ToArray();
        if (_stateMachine is null
            || _states is null
            || _adapters is null)
            throw new ArgumentException("State Machine was misconfigured");
        
        loggingProvider.LogDebug("Initializing states:");
        loggingProvider.LogDebug($"State initialized: Start (1/{_states.Count()+2})");
        foreach (var (state, counter) in _states.Select((state, counter) => (state, counter+2)))
        {
            await state.Initialize(ServiceProvider);
            loggingProvider.LogDebug($"State initialized: {state} ({counter}/{_states.Count()+2})");
        }
        loggingProvider.LogDebug($"State initialized: Final ({_states.Count()+2}/{_states.Count()+2})");
        loggingProvider.LogDebug($"All {_states.Count()+2} states successfully initialized ✅");

        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("Initializing adapters:");
        foreach (var (adapter, counter) in _adapters.Select((adapter, i) => (adapter, i+1)))
        {
            await adapter.CallInitialize(ServiceProvider);
            loggingProvider.LogDebug($"Adapter initialized: {adapter} ({counter}/{_adapters.Count()})");
        }
        
        loggingProvider.LogDebug($"All {_adapters.Count()} adapters successfully initialized ✅");
        loggingProvider.LogDebug("");
        loggingProvider.LogDebug("Setup complete, starting test now:");
        loggingProvider.LogDebug("---------------------------------");
        
        _startTime = DateTime.Now;
        await _stateMachine.Start(ServiceProvider);
        return ServiceProvider;
    }

    private bool _firstStep = true;
    private DateTime _testStart;
    internal async Task<StepResult> StepAsync(IServiceProvider serviceProvider)
    {
        if(_firstStep)
        {
            _firstStep = true;
            _testStart = DateTime.Now;
        }
            
        if (_stateMachine.CurrentXceptoState.Equals(_stateMachine.FinalXceptoState))
            return StepResult.Finished;

        if (DateTime.Now - _startTime >= _timeout.Total)
            return StepResult.Canceled;
        
        if (DateTime.Now - _testStart >= _timeout.Test)
            return StepResult.TestCanceled;

        await _stateMachine.TryTransition(serviceProvider);
        return StepResult.Continue;
    }

    internal async Task CleanupAsync(IServiceProvider serviceProvider)
    {
        var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
        var disposeProvider = serviceProvider.GetRequiredService<DisposeProvider>();
        loggingProvider.LogDebug("---------------------------------");
        loggingProvider.LogDebug("Test completed ✅");
        loggingProvider.LogDebug("");
        
        loggingProvider.LogDebug("Cleaning up:");
        foreach (var (adapter, counter) in _adapters.Select((adapter, i) => (adapter, i + 1)))
        {
            try
            {
                await adapter.CallCleanup(serviceProvider);
            }
            finally
            {
                disposeProvider?.DisposeAll();
            }
            loggingProvider.LogDebug($"Adapter cleanup: {adapter} ({counter}/{_adapters.Count()})");
        }
        loggingProvider.LogDebug($"All {_adapters.Count()} adapters were successfully cleaned up ✅");

        await _scenario.CallCleanup();
        
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

    internal TimeoutConfig GetTimeout() => _timeout;

    internal Func<IEnumerable<Task>> GetPropagatedTasksSupplier() => _propagatedTasksSupplier;
}
