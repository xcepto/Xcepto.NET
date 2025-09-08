
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xcepto
{
    public class TransitionBuilder
    {
        private AcceptanceStateMachine _stateMachine = new();
        private HashSet<XceptoAdapter> _adapters = new();
        private List<XceptoState> _states = new();
        private List<Task> _propagatedTasks = new();
        internal IEnumerable<Task> PropagatedTasks => _propagatedTasks;

        public void AddStep(XceptoState newState)
        {
            _states.Add(newState);
            _stateMachine.AddTransition(newState);
        }

        internal AcceptanceStateMachine Build()
        {
            _stateMachine.Seal();
            return _stateMachine;
        }

        internal IEnumerable<XceptoState> GetStates() => _states;
        internal IEnumerable<XceptoAdapter> GetAdapters() => _adapters;

        public TXceptoAdapter RegisterAdapter<TXceptoAdapter>(TXceptoAdapter adapter)
        where TXceptoAdapter: XceptoAdapter
        {
            adapter.AssignBuilder(this);
            _adapters.Add(adapter);
            return adapter;
        }

        internal void PropagateExceptions(Task task)
        {
            _propagatedTasks.Add(task);
        }
    }
}