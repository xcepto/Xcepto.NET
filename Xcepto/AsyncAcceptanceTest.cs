using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public class AsyncAcceptanceTest: AcceptanceTest
    {
        private Scenario _scenario;

        public AsyncAcceptanceTest(TimeSpan timeout, TransitionBuilder transitionBuilder, Scenario scenario) : base(timeout, transitionBuilder)
        {
            _scenario = scenario;
        }
        
        public async Task ExecuteTestAsync()
        {
            // Arrange
            IServiceCollection serviceCollection = await _scenario.Setup();
            var serviceProvider = await Arrange(serviceCollection);
            await _scenario.Initialize(serviceProvider);
            
            try
            {
                // Act
                DateTime startTime = DateTime.Now;
                await _stateMachine.Start(serviceProvider);
                while (DateTime.Now - startTime < _timeout)
                {
                    await _stateMachine.TryTransition(serviceProvider);

                    await Task.Delay(TimeSpan.FromSeconds(0.1f));
                }

                // Assert
                Assert(_stateMachine);
            }
            finally
            {
                await Cleanup(serviceProvider);
                await _scenario.Cleanup(serviceProvider);
            }
            
        }
    }
}