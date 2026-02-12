using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<IStateBuilderIdentity> _futureStates = new();
        private Dictionary<IStateBuilderIdentity, Func<XceptoState>> _stateProducers = new();

        public TransitionBuilder(Action<TransitionBuilder> arrange)
        {
            _arrange = arrange;
        }

        internal IEnumerable<Task> PropagatedTasks => _propagatedTasks;

        public void AddStep(XceptoState newState)
        {
            var stateBuilder = new AnonymousStateBuilderIdentity();
            _futureStates.Add(stateBuilder);
            _stateProducers[stateBuilder] = () => newState;
        }

        internal AcceptanceStateMachine Build()
        {
            _arrange(this);
            foreach (var futureState in _futureStates)
            {
                if (!_stateProducers.TryGetValue(futureState, out var producer) || producer is null)
                    throw new Exception("StateMachine construction failed: invalid state producer");
                var xceptoState = producer();
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

        public void AddFutureStep(Func<XceptoState> futureState, IStateBuilderIdentity stateBuilderIdentity)
        {
            if(!_stateProducers.ContainsKey(stateBuilderIdentity))
                _futureStates.Add(stateBuilderIdentity);
            _stateProducers[stateBuilderIdentity] = futureState;
        }
    }
}