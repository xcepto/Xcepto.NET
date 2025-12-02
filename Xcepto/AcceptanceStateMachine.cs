using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.States;

namespace Xcepto
{
    public class AcceptanceStateMachine
    {
        public AcceptanceStateMachine()
        {
            _startXceptoState = new UnconditionalXceptoState("Start");
            _currentXceptoState = _startXceptoState;
        }
        
        public void AddTransition(XceptoState newState)
        {
            _currentXceptoState.NextXceptoState = newState;
            _currentXceptoState = newState;
        }
        
        public string GetStatus()
        {
            return _currentXceptoState.Name;
        }

        readonly XceptoState _startXceptoState;
        XceptoState _currentXceptoState;
        XceptoState _finalXceptoState;
        
        public void Seal()
        {
            _finalXceptoState = new UnconditionalXceptoState("Final");
            _currentXceptoState.NextXceptoState = _finalXceptoState;
            _currentXceptoState = _startXceptoState;
        }
        public XceptoState CurrentXceptoState => _currentXceptoState;
        public XceptoState FinalXceptoState => _finalXceptoState;

        public async Task TryTransition(IServiceProvider serviceProvider)
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

        public async Task Start(IServiceProvider serviceProvider)
        {
            await _currentXceptoState.OnEnter(serviceProvider);

        }
    }
}