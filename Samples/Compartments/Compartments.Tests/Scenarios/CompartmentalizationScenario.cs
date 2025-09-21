using Compartments.Tests.Dependencies;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Compartments.Tests.Scenarios;

public class CompartmentalizationScenario: XceptoScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        return Task.FromResult(new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<Service2>()
            .AddSingleton<Service1>()
            .AddTransient<Dependency1>()
        );
    }
}