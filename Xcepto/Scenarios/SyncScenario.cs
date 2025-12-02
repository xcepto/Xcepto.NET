using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Xcepto.Scenarios;

public class SyncScenario: BaseScenario
{
    protected virtual IServiceCollection Setup() => new ServiceCollection()
        .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>();

    protected virtual void Initialize(IServiceProvider serviceProvider) { }

    protected virtual void Cleanup(IServiceProvider serviceProvider) { }

    protected override Task<IServiceProvider> BaseSetup()
    {
        var serviceCollection = Setup();
        return Task.FromResult<IServiceProvider>(serviceCollection.BuildServiceProvider());
    }

    protected override Task BaseInitialize(IServiceProvider serviceProvider) {
        Initialize(serviceProvider);
        return Task.CompletedTask;
    }

    protected override Task BaseCleanup(IServiceProvider serviceProvider)
    {
        Cleanup(serviceProvider);
        return Task.CompletedTask;
    }
}