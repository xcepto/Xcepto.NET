
using System.Collections.Generic;

namespace Xcepto
{
    public class TransitionBuilder
    {
        private AcceptanceStateMachine _stateMachine = new();
        private HashSet<XceptoAdapter> _adapters = new();
        
        public void AddStep(XceptoState newState)
        {
            _stateMachine.AddTransition(newState);
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
            adapter.AssignBuilder(this);
            _adapters.Add(adapter);
            return adapter;
        }
    }
}