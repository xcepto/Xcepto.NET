using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class AcceptanceTest
    {
        protected AcceptanceStateMachine _stateMachine;
        protected TimeSpan _timeout;
        protected IEnumerable<XceptoAdapter> _adapters;

        public AcceptanceTest(TimeSpan timeout, TransitionBuilder transitionBuilder)
        {
            _stateMachine = transitionBuilder.Build();
            _timeout = timeout;
            _adapters = transitionBuilder.GetAdapters();
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

            return serviceProvider;
        }

        protected async Task Cleanup()
        {
            foreach (var xceptoAdapter in _adapters)
            {
                await xceptoAdapter.CallCleanup();
            }
        }
    }
}
