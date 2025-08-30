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
            yield return _enumeratedScenario.Setup(serviceCollection);
            var arrangeTask = Arrange(serviceCollection);
            arrangeTask.Wait();
            var serviceProvider = arrangeTask.Result;

            var initializeTask = _enumeratedScenario.Initialize(serviceProvider);
            initializeTask.Wait();

            try
            {
                // Act
                DateTime startTime = DateTime.Now;
                var startTask = _stateMachine.Start(serviceProvider);
                startTask.Wait();
                while (DateTime.Now - startTime < _timeout)
                {
                    var transitionTask = _stateMachine.TryTransition(serviceProvider);
                    transitionTask.Wait();

                    yield return EnumeratedWait();
                }

                // Assert
                Assert(_stateMachine);
            }
            finally
            {
                var cleanupTask = Cleanup(serviceProvider);
                cleanupTask.Wait();

                var scenarioCleanupTask = _enumeratedScenario.Cleanup(serviceProvider);
                scenarioCleanupTask.Wait();
            }
        }

        protected abstract IEnumerator EnumeratedWait();
    }
}