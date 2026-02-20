using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.States;
using Xcepto.Util;

namespace Xcepto.Internal
{
    internal class AcceptanceStateMachine
    {
        internal AcceptanceStateMachine()
        {
            _startXceptoState = new UnconditionalXceptoState("Start");
            _currentXceptoState = _startXceptoState;
        }
        
        internal void AddTransition(XceptoState newState)
        {
            _currentXceptoState.NextXceptoState = newState;
            _currentXceptoState = newState;
        }

        readonly XceptoState _startXceptoState;
        XceptoState _currentXceptoState;
        XceptoState _finalXceptoState;
        
        internal void Seal()
        {
            _finalXceptoState = new UnconditionalXceptoState("Final");
            _currentXceptoState.NextXceptoState = _finalXceptoState;
            _currentXceptoState = _startXceptoState;
        }
        internal XceptoState CurrentXceptoState => _currentXceptoState;
        internal XceptoState FinalXceptoState => _finalXceptoState;

        internal async Task TryTransition(IServiceProvider serviceProvider)
        {
            if(_currentXceptoState.NextXceptoState is null)
                return; // no more transitions

            var allConditionsMet = false;

            try
            {
                allConditionsMet = await _currentXceptoState.EvaluateConditionsForTransition(serviceProvider);
            }
            catch (Exception e)
            {
                throw new StateTransitionException($"State failed to transition: [{_currentXceptoState.Name}] ({_currentXceptoState.GetType().Name}, state #{DetermineCurrentStateNumber()})").Promote(e);
            }

            if (allConditionsMet)
            {
                _currentXceptoState = _currentXceptoState.NextXceptoState;
                try
                {
                    await _currentXceptoState.OnEnter(serviceProvider);
                }
                catch (Exception e)
                {
                    throw new StateEnterException($"State failed on enter: [{_currentXceptoState.Name}] ({_currentXceptoState.GetType().Name}, state #{DetermineCurrentStateNumber()})").Promote(e);
                }
                var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
                loggingProvider.LogDebug($"Transitioned to: {_currentXceptoState.Name}");
            }
        }

        private int DetermineCurrentStateNumber()
        {
            int count = 0;
            XceptoState? current = _startXceptoState;
            while (true)
            {
                if (current is null)
                    throw new Exception("Failed determining the current state number");
                if(current == _currentXceptoState)
                    break;
                current = current?.NextXceptoState;
                count++;
            }

            return count;
        }

        internal async Task Start(IServiceProvider serviceProvider)
        {
            await _currentXceptoState.OnEnter(serviceProvider);

        }
    }
}