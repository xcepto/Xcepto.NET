using System;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class EnumeratedAcceptanceTest: AcceptanceTest
    {
        private EnumeratedScenario _enumeratedScenario;

        public EnumeratedAcceptanceTest(TimeSpan timeout, TransitionBuilder transitionBuilder, EnumeratedScenario enumeratedScenario) 
            : base(timeout, transitionBuilder)
        {
            _enumeratedScenario = enumeratedScenario;
        }

        public IEnumerator ExecuteTestEnumerated()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            yield return _enumeratedScenario.CallSetup(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var arrangeTask = Arrange(serviceProvider);
            arrangeTask.Wait();

            var initializeTask = _enumeratedScenario.CallInitialize(serviceProvider);
            initializeTask.Wait();

            try
            {
                // Act
                DateTime startTime = DateTime.Now;
                var startTask = StateMachine.Start(serviceProvider);
                startTask.Wait();
                while (DateTime.Now - startTime < Timeout)
                {
                    if(StateMachine.CurrentXceptoState.Equals(StateMachine.FinalXceptoState))
                        yield break;
                    
                    var transitionTask = StateMachine.TryTransition(serviceProvider);
                    transitionTask.Wait();

                    yield return EnumeratedWait();
                }

                // Assert
                Assert(StateMachine);
            }
            finally
            {
                var cleanupTask = Cleanup(serviceProvider);
                cleanupTask.Wait();

                var scenarioCleanupTask = _enumeratedScenario.CallCleanup(serviceProvider);
                scenarioCleanupTask.Wait();
            }
        }

        protected abstract IEnumerator EnumeratedWait();
    }
}