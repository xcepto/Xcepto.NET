using Compartments.Tests.Dependencies;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Compartments.Tests.Scenarios;

public class SharedSyncScenario: SyncScenario
{
    protected override IServiceCollection Setup()
    {
        return new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<Service2>()
            .AddSingleton<Service1>()
            .AddSingleton<PersonalDependency>()
            .AddSingleton<SharedDependency>();
    }
}