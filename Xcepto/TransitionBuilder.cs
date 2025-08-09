
using System.Collections.Generic;

namespace Xcepto
{
    public class TransitionBuilder
    {
        private AcceptanceStateMachine _stateMachine = new AcceptanceStateMachine();
        private HashSet<XceptoAdapter> _adapters = new HashSet<XceptoAdapter>();
        
        public TransitionBuilder AddStep(XceptoState newState)
        {
            _stateMachine.AddTransition(newState);
            return this;
        }

        internal AcceptanceStateMachine Build()
        {
            _stateMachine.Seal();
            return _stateMachine;
        }

        internal IEnumerable<XceptoAdapter> GetAdapters() => _adapters;

        public TXceptoAdapter RegisterAdapter<TXceptoAdapter>(TXceptoAdapter adapter)
        where TXceptoAdapter: XceptoAdapter
        {
            _adapters.Add(adapter);
            return adapter;
        }
    }
}