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
        var service0Compartment = Compartment.From(new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            )
            .ExposeService<ILoggingProvider>()
            .Build();
        var service1Compartment = Compartment.From(new ServiceCollection()
            .AddSingleton<Service1>()
            .AddSingleton<Dependency1>()
            )
            .ExposeService<Service1>()
            .Build();
        var service2Compartment = Compartment.From(new ServiceCollection()
            .AddSingleton<Service2>()
            .AddSingleton<Dependency1>()
            )
            .ExposeService<Service2>()
            .Build();
        return Task.FromResult(new []{service0Compartment, service1Compartment, service2Compartment}.AsEnumerable());
    }
}