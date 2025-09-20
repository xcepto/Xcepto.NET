using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;

namespace Xcepto
{
    public abstract class AcceptanceTest
    {
        protected readonly AcceptanceStateMachine StateMachine;
        protected TimeSpan Timeout;
        private readonly IEnumerable<XceptoAdapter> _adapters;
        private readonly IEnumerable<XceptoState> _states;

        protected AcceptanceTest(TimeSpan timeout, TransitionBuilder transitionBuilder)
        {
            StateMachine = transitionBuilder.Build();
            Timeout = timeout;
            _adapters = transitionBuilder.GetAdapters();
            _states = transitionBuilder.GetStates();
        }

        protected async Task Arrange(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
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
        }
        
        protected static void Assert(AcceptanceStateMachine stateMachine)
        {
            if (!stateMachine.CurrentXceptoState.Equals(stateMachine.FinalXceptoState))
                throw new Exception($"{stateMachine.CurrentXceptoState} did not equal {stateMachine.FinalXceptoState}");
        }

        protected async Task Cleanup(IServiceProvider serviceProvider)
        {
            var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
            loggingProvider.LogDebug("Cleaning up:");
            foreach (var (adapter, counter) in _adapters.Select((adapter, i) => (adapter, i + 1)))
            {
                await adapter.CallCleanup(serviceProvider);
                loggingProvider.LogDebug($"Adapter cleanup: {adapter} ({counter}/{_adapters.Count()})");
            }
            loggingProvider.LogDebug($"All {_adapters.Count()} adapters were successfully cleaned up ✅");
        }
    }
}
