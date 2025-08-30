using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
        
        protected static void Assert(AcceptanceStateMachine stateMachine)
        {
            if (!stateMachine.CurrentXceptoState.Equals(stateMachine.FinalXceptoState))
                throw new Exception($"{stateMachine.CurrentXceptoState} did not equal {stateMachine.FinalXceptoState}");
        }

        protected async Task<IServiceProvider> Arrange(IServiceCollection serviceCollection)
        {
            var xceptoAdapters = _adapters as XceptoAdapter[] ?? _adapters.ToArray();
            foreach (var adapter in xceptoAdapters)
            {
                await adapter.CallAddServices(serviceCollection);
            }
            
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            foreach (var adapter in xceptoAdapters)
            {
                await adapter.CallInitialize(serviceProvider);
            }
            
            foreach (var xceptoState in _states)
            {
                await xceptoState.Initialize(serviceProvider);
            }

            return serviceProvider;
        }

        protected async Task Cleanup(IServiceProvider serviceProvider)
        {
            foreach (var xceptoAdapter in _adapters)
            {
                await xceptoAdapter.CallCleanup(serviceProvider);
            }
        }
    }
}
