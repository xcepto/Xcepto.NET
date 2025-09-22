using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;

namespace Xcepto.Scenarios;

public class CompartmentalizedXceptoScenario: BaseScenario
{
    protected virtual Task<IEnumerable<Compartment>> Setup()
    {
        var firstCompartmentServiceCollection = new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>();
        Compartment firstCompartment = Compartment.From(firstCompartmentServiceCollection)
            .ExposeService<ILoggingProvider>()
            .Build();
        return Task.FromResult<IEnumerable<Compartment>>([firstCompartment]);
    }

    protected virtual Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected virtual Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;

    private List<Compartment> _registeredCompartments = new(); 

    protected override async Task<IServiceProvider> BaseSetup()
    {
        ServiceCollection primaryCollection = new ServiceCollection();
        var compartments = await Setup();
        var enumerable = compartments as Compartment[] ?? compartments.ToArray();
        foreach (var compartment in enumerable)
        {
            _registeredCompartments.Add(compartment);
            var exposedServices = compartment.GetExposedServices();
            foreach (var exposedService in exposedServices)
            {
                primaryCollection.AddSingleton(exposedService.Type, _ => exposedService.InstanceSupplier());
            }
        }

        var primaryProvider = primaryCollection.BuildServiceProvider();
        foreach (var compartment in enumerable)
        {
            compartment.Activate(primaryProvider);
        }
        return primaryProvider;
    }

    protected override Task BaseInitialize(IServiceProvider serviceProvider) => Initialize(serviceProvider);

    protected override Task BaseCleanup(IServiceProvider serviceProvider) => Cleanup(serviceProvider);
}