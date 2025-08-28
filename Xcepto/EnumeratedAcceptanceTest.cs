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
            try
            {
                // Arrange
                var serviceCollection = new ServiceCollection();
                yield return _enumeratedScenario.Setup(serviceCollection);
                var providerTask = Arrange(serviceCollection);
                providerTask.Wait();
                var serviceProvider = providerTask.Result;

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
                var cleanupTask = Cleanup();
                cleanupTask.Wait();
            }
        }

        protected abstract IEnumerator EnumeratedWait();
    }
}