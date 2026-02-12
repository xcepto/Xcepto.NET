using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Samples.Rest.API.Tests.Images;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;
using Xcepto.Testcontainers.Extensions;
using Xcepto.Testcontainers.Interfaces;

namespace Samples.Rest.API.Tests.Scenarios;

public class MockedTokenScenario(byte[] tokenHash) : XceptoScenario
{
    private IContainer? _api;
    public int ApiPort { get; private set; }

    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddTestcontainersSupport()
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder
        .Do(async x =>
        {
            var identifier = Convert.ToHexString(Guid.NewGuid().ToByteArray());
            var support = x.GetRequiredService<ITestContainerSupport>();
            
            _api = new ContainerBuilder(await ImageSingleton.CreateApiImageOnce())
                .WithName($"api-{identifier}")
                .WithPortBinding(8080, true)
                .WithEnvironment("TOKENHASH", Convert.ToHexString(tokenHash))
                .WithOutputConsumer(support.CreateOutputConsumer("api", true))
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(x => x
                        .ForPath("/api/ping")
                        .ForPort(8080)
                    )
                )
                .WithLogger(support.CreateLogger("api", false))
                .Build();

            await _api.StartAsync();
            
            ApiPort = _api.GetMappedPublicPort(8080);
        })
        .Build();
}