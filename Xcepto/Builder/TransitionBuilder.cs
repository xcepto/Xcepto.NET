using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xcepto.Adapters;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.States;

namespace Xcepto.Builder
{
    public class TransitionBuilder: IStateMachineBuilder
    {
        private AcceptanceStateMachine _stateMachine = new();
        private HashSet<XceptoAdapter> _adapters = new();
        private List<XceptoState> _states = new();
        private List<Task> _propagatedTasks = new();
        private Action<TransitionBuilder> _arrange;
        private List<Func<XceptoState>> _futureStates = new();

        public TransitionBuilder(Action<TransitionBuilder> arrange)
        {
            _arrange = arrange;
        }

        internal IEnumerable<Task> PropagatedTasks => _propagatedTasks;

        public void AddStep(XceptoState newState)
        {
            _futureStates.Add(() => newState);
        }

        internal AcceptanceStateMachine Build()
        {
            _arrange(this);
            foreach (var futureState in _futureStates)
            {
                var xceptoState = futureState();
                xceptoState.AssignBuilder(this);
                _states.Add(xceptoState);
                _stateMachine.AddTransition(xceptoState);
            }
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

        public void AddFutureStep(Func<XceptoState> futureState)
        {
            _futureStates.Add(futureState);
        }
    }
}