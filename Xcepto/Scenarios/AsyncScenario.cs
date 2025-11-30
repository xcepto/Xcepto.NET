using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Xcepto;

public class AsyncScenario: BaseScenario
{
    protected virtual Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection()
        .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>());

    protected virtual Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected virtual Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override async Task<IServiceProvider> BaseSetup()
    {
        var serviceCollection = await Setup();
        return serviceCollection.BuildServiceProvider();
    }

    protected override Task BaseInitialize(IServiceProvider serviceProvider) => Initialize(serviceProvider);

    protected override Task BaseCleanup(IServiceProvider serviceProvider) => Cleanup(serviceProvider);
}