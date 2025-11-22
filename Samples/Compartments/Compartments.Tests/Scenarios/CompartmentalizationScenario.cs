using Compartments.Tests.Dependencies;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Compartments.Tests.Scenarios;

public class CompartmentalizationScenario: CompartmentalizedXceptoScenario
{
    protected override Task<IEnumerable<Compartment>> Setup()
    {
        var shared = Compartment.From(new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<SharedDependency>()
            )
            .ExposeService<SharedDependency>()
            .ExposeService(typeof(ILoggingProvider))
            .Build();
        var service1 = Compartment.From(new ServiceCollection()
            .AddSingleton<Service1>()
            .AddSingleton<PersonalDependency>()
            )
            .DependsOn<SharedDependency>()
            .ExposeService<Service1>()
            .Build();
        var service2 = Compartment.From(new ServiceCollection()
            .AddSingleton<Service2>()
            .AddSingleton<PersonalDependency>()
            )
            .DependsOn(typeof(SharedDependency))
            .ExposeService<Service2>()
            .Build();
        return Task.FromResult(new []{shared, service1, service2}.AsEnumerable());
    }
}