using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto;

public abstract class SequentialScenario: BaseScenario
{
    private bool _initialized = false;
    private bool _setup = false;
    private IServiceProvider? _serviceProvider;
    private IServiceCollection? _serviceCollection;

    protected virtual Task<IServiceCollection> OneTimeSetup() => Task.FromResult<IServiceCollection>(new ServiceCollection());
    protected virtual Task OneTimeTeardown(IServiceProvider serviceProvider) => Task.CompletedTask;

    public async Task TearDown()
    {
        if (_serviceProvider is null)
            throw new Exception("Sequential Scenario was not initialized yet");
        await OneTimeTeardown(_serviceProvider);
    }

    protected override async Task<IServiceProvider> BaseSetup()
    {
        if (_setup)
            return _serviceProvider!;
        _serviceCollection = await OneTimeSetup();
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _setup = true;
        return _serviceProvider;
    }
    
    protected override Task BaseInitialize(IServiceProvider serviceProvider)
    {
        if(_initialized)
            return Task.CompletedTask;
        
        _initialized = true;
        return Task.CompletedTask;
    }

    protected override Task BaseCleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}