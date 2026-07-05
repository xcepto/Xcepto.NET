using Compartments.Tests.Adapters;
using Compartments.Tests.Scenarios;
using Compartments.Tests.Service;
using Xcepto;

namespace Compartments.Tests.Test;

[TestFixture]
public class CompartmentEntryPointTests
{
    [Test]
    public async Task WorkerServiceActsAutonomously()
    {
        var firstGuid = Guid.NewGuid();
        var secondGuid = Guid.NewGuid();
        var compartment = "client";
        await XceptoTest.Given(new EntryPointScenario(), builder =>
        {
            var adapter = builder.RegisterAdapter(new CompartmentAccessAdapter());

            adapter.CompartmentServiceAction<ClientService>(compartment,
                clientService => clientService.CreateTask(firstGuid));
            adapter.CompartmentServiceAction<ClientService>(compartment, 
                clientService => clientService.CreateTask(secondGuid));

            adapter.CompartmentExpectation<ClientService>(compartment, clientService => 
                clientService.CheckCompletion(firstGuid));
            adapter.CompartmentExpectation<ClientService>(compartment, clientService => 
                clientService.CheckCompletion(secondGuid));
        });
    }
}