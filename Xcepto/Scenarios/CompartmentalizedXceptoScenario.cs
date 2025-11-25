using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Repositories;

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


    protected override async Task<IServiceProvider> BaseSetup()
    {
        ServiceCollection primaryCollection = new ServiceCollection();
        CompartmentRepository compartmentRepository = new CompartmentRepository();
        primaryCollection.AddSingleton<CompartmentRepository>(compartmentRepository);
        
        var compartments = await Setup();
        var enumerable = compartments as Compartment[] ?? compartments.ToArray();
        foreach (var compartment in enumerable)
        {
            compartmentRepository.AddCompartment(compartment);
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
            if (compartment.EntryPoint is not null)
                PropagateExceptions(Task.Run(() => compartment.EntryPoint(compartment.Services)));
        }
        return primaryProvider;
    }

    protected override Task BaseInitialize(IServiceProvider serviceProvider) => Initialize(serviceProvider);

    protected override Task BaseCleanup(IServiceProvider serviceProvider) => Cleanup(serviceProvider);
}