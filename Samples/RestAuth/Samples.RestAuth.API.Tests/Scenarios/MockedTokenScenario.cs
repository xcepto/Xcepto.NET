using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;
using Xcepto.Testcontainers.Extensions;
using Xcepto.Testcontainers.Interfaces;

namespace Samples.RestAuth.API.Tests.Scenarios;

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
            var support = x.GetRequiredService<ITestContainerSupport>();

            var apiImage = new ImageFromDockerfileBuilder()
                .WithName("samples-restauth-api:test")
                .WithCleanUp(false)
                .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), Path.Combine("Samples", "RestAuth"))
                .WithDockerfile("Samples.RestAuth.API/Dockerfile")
                .WithLogger(support.CreateLogger("api-image-builder", false))
                .Build();

            await apiImage.CreateAsync();
            
            _api = new ContainerBuilder(apiImage)
                .WithName("api")
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