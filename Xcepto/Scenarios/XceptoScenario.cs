using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Provider;

namespace Xcepto.Scenarios
{
    public abstract class XceptoScenario
    {
        private TransitionBuilder? _builder;
        private bool _initialized = false;
        private bool _setup = false;
        private IServiceProvider? _serviceProvider;

        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }

        protected abstract ScenarioSetup Setup(ScenarioSetupBuilder builder);

        protected virtual ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder.Build();

        protected virtual ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder.Build();

        public Task Teardown()
        {
            Cleanup(new ScenarioCleanupBuilder(_serviceProvider!));
            return Task.CompletedTask;
        }
        
        
        internal async Task<IServiceProvider> CallSetup() {
            
            if (_setup)
                return _serviceProvider!;

            var scenarioSetup = Setup(new ScenarioSetupBuilder());
            
            foreach (var function in scenarioSetup.DoTasks)
            {
                await function();
            }
            _serviceProvider = scenarioSetup.ServiceCollection.BuildServiceProvider();
            var disposeProvider = _serviceProvider.GetRequiredService<DisposeProvider>();
            foreach (var disposableType in scenarioSetup.Disposables)
            {
                var service = _serviceProvider.GetService(disposableType);
                if (service is null)
                    throw new Exception($"Disposable type {disposableType.FullName} was not found in service collection");

                if (service is not IDisposable disposable)
                    throw new Exception($"Type {disposableType.FullName} was not IDisposable");
                
                disposeProvider.Add(disposable);
            }
            _setup = true;
            return _serviceProvider;
        }
        internal async Task CallInitialize()
        {
            if(_initialized)
                return;

            var scenarioInitialization = Initialize(new ScenarioInitializationBuilder(_serviceProvider!));
            foreach (var function in scenarioInitialization.DoTasks)
            {
                await function();
            }
            foreach (var function in scenarioInitialization.FireAndForgetTasks)
            {
                _builder!.PropagateExceptions(function());
            }
            _initialized = true;
        }

        internal async Task CallCleanup() {
            var scenarioCleanup = Cleanup(new ScenarioCleanupBuilder(_serviceProvider!));
            foreach (var function in scenarioCleanup.DoTasks)
            {
                try
                {
                    await function();
                }
                finally
                {
                    var disposeProvider = _serviceProvider?.GetRequiredService<DisposeProvider>();
                    disposeProvider?.DisposeAll();
                }
            }
        }
    }
}
