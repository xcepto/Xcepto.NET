using System.Collections;
using EnumeratedExecutionTests.Repositories;
using EnumeratedExecutionTests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace EnumeratedExecutionTests.Scenario;

public class ExampleEnumeratedScenario: EnumeratedScenario
{
    protected override IEnumerator Setup(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<ServiceA>()
            .AddSingleton<ServiceB>()
            .AddSingleton<Repository>();
        yield return null;
    }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}