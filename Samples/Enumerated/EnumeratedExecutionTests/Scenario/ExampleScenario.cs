using System.Collections;
using EnumeratedExecutionTests.Repositories;
using EnumeratedExecutionTests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace EnumeratedExecutionTests.Scenario;

public class ExampleScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<ServiceA>()
            .AddSingleton<ServiceB>()
            .AddSingleton<Repository>()
        )
        .Build();
}