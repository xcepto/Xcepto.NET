using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Provider;
using Xcepto.Repositories;

namespace Xcepto.Scenarios;

public class CompartmentalizedXceptoScenario: XceptoScenario
{
    private Compartment[]? _compartments;

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


    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder)
    {
        ServiceCollection primaryCollection = new ServiceCollection();
        CompartmentRepository compartmentRepository = new CompartmentRepository();
        primaryCollection
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<DisposeProvider>()
            .AddSingleton<CompartmentRepository>(compartmentRepository);

        var setupTask = Setup();
        var compartments = setupTask.GetAwaiter().GetResult();
        _compartments = compartments as Compartment[] ?? compartments.ToArray();
        foreach (var compartment in _compartments)
        {
            compartmentRepository.AddCompartment(compartment);
            var exposedServices = compartment.GetExposedServices();
            foreach (var exposedService in exposedServices)
            {
                primaryCollection.AddSingleton(exposedService.Type, _ => exposedService.InstanceSupplier());
            }
        }

        builder.AddServices(services => primaryCollection);
        return builder.Build();
    }

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) {
        builder.Do(Initialize);
        foreach (var compartment in _compartments!)
        {
            builder.Do(serviceProvider => compartment.Activate(serviceProvider));
            if (compartment.EntryPoint is not null)
                builder.FireAndForget(_ => Task.Run(() => compartment.EntryPoint(compartment.Services)));
        }
        return builder.Build();
    }

    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder
        .Do(Cleanup)
        .Build();
}