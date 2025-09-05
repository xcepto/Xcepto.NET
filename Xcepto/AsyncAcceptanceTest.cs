using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;

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
            var loggingProvider = serviceCollection.BuildServiceProvider().GetRequiredService<ILoggingProvider>();
            loggingProvider.LogDebug("Setting up acceptance test");
            loggingProvider.LogDebug("");
            loggingProvider.LogDebug("Added scenario services successfully ✅");
            loggingProvider.LogDebug("");
            var serviceProvider = await Arrange(serviceCollection);
            loggingProvider.LogDebug("");
            await _scenario.Initialize(serviceProvider);
            loggingProvider.LogDebug("Initialized scenario successfully ✅");
            loggingProvider.LogDebug("");
            loggingProvider.LogDebug("Setup complete, starting test now:");
            loggingProvider.LogDebug("---------------------------------");
            
            try
            {
                // Act
                await StateMachine.Start(serviceProvider);
                while (!StateMachine.CurrentXceptoState.Equals(StateMachine.FinalXceptoState))
                {
                    await StateMachine.TryTransition(serviceProvider);

                    await Task.Delay(TimeSpan.FromSeconds(0.1f));
                }

                // Assert
                Assert(StateMachine);
            }
            finally
            {
                loggingProvider.LogDebug("---------------------------------");
                loggingProvider.LogDebug("Test completed ✅");
                loggingProvider.LogDebug("");
                await Cleanup(serviceProvider);
                await _scenario.Cleanup(serviceProvider);
            }
            
        }
    }
}