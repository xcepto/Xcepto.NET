using System.Collections;
using EnumeratedExecutionTests.Repositories;
using EnumeratedExecutionTests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace EnumeratedExecutionTests.Scenario;

public class ExampleScenario: AsyncScenario
{
    protected override Task<IServiceProvider> BaseSetup()
    {
        return Task.FromResult<IServiceProvider>(
            new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<ServiceA>()
            .AddSingleton<ServiceB>()
            .AddSingleton<Repository>()
            .BuildServiceProvider());
    }

    protected override Task BaseInitialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task BaseCleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}