using Compartments.Tests.Dependencies;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Compartments.Tests.Scenarios;

public class SharedSyncScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<Service2>()
            .AddSingleton<Service1>()
            .AddSingleton<PersonalDependency>()
            .AddSingleton<SharedDependency>()
        )
        .Build();
}