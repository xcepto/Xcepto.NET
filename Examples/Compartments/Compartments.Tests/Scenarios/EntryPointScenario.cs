using Compartments.Tests.Repositories;
using Compartments.Tests.Service;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace Compartments.Tests.Scenarios;

public class EntryPointScenario: CompartmentalizedXceptoScenario
{
    protected override Task<IEnumerable<Compartment>> Setup()
    {
        IServiceCollection outerCollection = new ServiceCollection()
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
            .AddSingleton<ResultRepository>()
            .AddSingleton<TaskRepository>();
        var outerCompartment = Compartment.From(outerCollection)
            .ExposeService<ResultRepository>()
            .ExposeService<TaskRepository>()
            .ExposeService<ILoggingProvider>()
            .Build();


        IServiceCollection workerCollection = new ServiceCollection()
            .AddSingleton<WorkerService>();
        var workerCompartment = Compartment.From(workerCollection)
            .DependsOn<ResultRepository>()
            .DependsOn<TaskRepository>()
            .SetEntryPoint<WorkerService>() // both options are possible
            .SetEntryPoint(typeof(WorkerService)) // both options are possible
            .Build();

        IServiceCollection clientCollection = new ServiceCollection()
            .AddSingleton<ClientService>();
        var clientCompartment = Compartment.From(clientCollection)
            .DependsOn<ResultRepository>()
            .DependsOn<TaskRepository>()
            .Identify("client")
            .Build();
        
        return Task.FromResult(new []{outerCompartment, workerCompartment, clientCompartment}.AsEnumerable());
    }
}