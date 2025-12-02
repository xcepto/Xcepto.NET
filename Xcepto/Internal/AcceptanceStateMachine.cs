using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.States;

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
        
        internal string GetStatus()
        {
            return _currentXceptoState.Name;
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

            var allConditionsMet = await _currentXceptoState.EvaluateConditionsForTransition(serviceProvider);

            if (allConditionsMet)
            {
                _currentXceptoState = _currentXceptoState.NextXceptoState;
                await _currentXceptoState.OnEnter(serviceProvider);
                var loggingProvider = serviceProvider.GetRequiredService<ILoggingProvider>();
                loggingProvider.LogDebug($"Transitioned to: {_currentXceptoState.Name}");
            }
        }

        internal async Task Start(IServiceProvider serviceProvider)
        {
            await _currentXceptoState.OnEnter(serviceProvider);

        }
    }
}