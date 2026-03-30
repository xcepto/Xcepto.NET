# Xcepto.NET

[![test pipeline](https://github.com/xcepto/Xcepto.NET/actions/workflows/cd.yaml/badge.svg)](https://github.com/xcepto/Xcepto.NET/actions/workflows/cd.yaml)
[![codecov](https://codecov.io/gh/xcepto/Xcepto.NET/graph/badge.svg?token=LG0UCC95K9)](https://codecov.io/gh/xcepto/Xcepto.NET)
[![semantic-release: conventional-commits](https://img.shields.io/badge/semantic--release-conventional--commits-e10079?logo=semantic-release)](https://www.conventionalcommits.org/en/v1.0.0/)
[![dotnet: netstandard2.0](https://img.shields.io/badge/compatibility-%E2%89%A5%20netstandard2.0-5632d5?logo=dotnet)](https://www.nuget.org/packages/Xcepto#supportedframeworks-body-tab)


Xcepto is a declarative **system-test** framework for distributed systems that **replaces** manual retries, sleeping and exception handling with a **state-machine**-based execution model, aligned with [Given-When-Then](https://dannorth.net/blog/introducing-bdd/#atm-example) semantics.

## Why Xcepto?

Traditional system tests rely on implicit timing assumptions that lead to non-deterministic results. Artificial workarounds include retries and gates. 

This becomes especially problematic in distributed systems, where events are asynchronous and ordering is not guaranteed. Forcing determinism with ad-hoc workarounds pollutes behavior specification into an unmaintainable mess.

Xcepto eliminates these issues by executing tests as condition-driven state machines, removing the need for timing assumptions entirely — making it a natural fit for systems built on asynchronous messaging as well as synchronous request-response interactions.

## Mental model

Test steps are **not executed immediately** when called!
They are compiled into a **state machine**, 
where **transition** depends on the specified **conditions**.

The states are linked in a chain:

```mermaid
graph LR
    Start --> First --> Second --> Third --> Final
```

The test passes, if the `Final` state was reached before a **timeout**. 

## Getting Started

```bash
dotnet add package Xcepto.NET
```

### Given

`Xcepto.Given` introduces a specification environment based on a scenario.
```csharp
var scenario = new YourScenario();
await XceptoTest.Given(scenario, builder =>
{
    // Declare test behaviour here
}
```

`Scenario` classes specify instructions to setup and prepare the system under test.

This snippet is runnable by itself. 

The next step is to specify test behavior using `Adapters`.

## Adapters

Raw Xcepto itself is just a runtime and lifecycle manager for tests.
Adapters actually integrate technologies into the Xcepto ecosystem.

`SSR` and `REST` have HTTP as interfacing boundary making them ideal targets for official adapters.

Here are some examples:

### Server Side Rendering (SSR)
```csharp
await XceptoTest.Given(scenario, builder =>
{
    var ssr = builder.SsrAdapterBuilder()
        .WithBaseUrl(new Uri($"http://localhost:{scenario.GuiPort}"))
        .Build();

    ssr.Post("/auth/register")
        .WithFormContent(new RegisterRequest(username, password).ToForm())
        .AssertSuccess()
        .AssertThatResponseContentString(Does.Not.Contain("id=\"errors\""));
    
    ssr.Post("/auth/login")
        .WithFormContent(new LoginRequest(username, password).ToForm())
        .AssertSuccess()
        .AssertThatResponseContentString(Does.Not.Contain("id=\"errors\""));

    ssr.Get("/")
        .AssertThatResponseContentString(Does.Contain(username));
});
```


### Combined REST & SSR
```csharp
await XceptoTest.Given(scenario, builder =>
{
    var ssr = builder.SsrAdapterBuilder()
        .WithBaseUrl(scenario.GuiAddress)
        .Build();

    var rest = builder.RestAdapterBuilder()
        .WithBaseUrl(scenario.ApiAddress)
        .Build();
                
    var tokenResponse = ssr.Post($"/token/create")
        .WithFormContent(new ProjectTokenCreateRequest("new token").ToForm())
        .AssertSuccess()
        .PromiseResponse();        
    
    rest.Post($"/api/env/create")
        .WithBearerTokenClient(() => ExtractToken(tokenResponse.Resolve()))
        .AssertSuccess();
});
```


### Custom adapters

Predefined adapters cover common cases. For complex systems (e.g. message-driven architectures like RabbitMQ and Kafka), custom adapters provide far more flexibility and control.

<details>
<summary><strong>Advanced: Build your own adapter</strong></summary>

Here is an example from my [HiveShard](https://hiveshard.massivecreationlab.com) project. The adapter integrates client compartments of the system and enables executing actions and setting exceptions on its behalf.

```csharp
public class HiveShardClientAdapter: XceptoAdapter
{
    private readonly CompartmentIdentifier _compartmentIdentifier;

    public HiveShardClientAdapter(HiveShardClient client)
    {
        _compartmentIdentifier = new CompartmentIdentifier(client.UserId, CompartmentType.Client);
    }

    public void Action(Func<IClientTunnel, Task> clientAction)
    {
        AddStep(new CompartmentalizedServiceBasedActionState<IClientTunnel>("Client Action", _compartmentIdentifier, clientAction)); 
    }

    public void Expect<T>(Predicate<T> expectation)
        where T: IEvent
    {
        AddStep(new CompartmentalizedClientExpectationState<T>($"Client Expectation of {typeof(T)}", _compartmentIdentifier, expectation));
    }
}
```


The states also need to be defined. They represent a lazy action or expectation.
They are only executed once this state has been reached in the chain.

```csharp
public class CompartmentalizedServiceBasedActionState<T>: XceptoState
    where T: class
{
    private readonly Func<T, Task> _action;
    private readonly CompartmentIdentifier _compartmentIdentifier;

    public CompartmentalizedServiceBasedActionState(string name, 
        CompartmentIdentifier compartmentIdentifier, Func<T, Task> action) : base(name)
    {
        _compartmentIdentifier = compartmentIdentifier;
        this._action = action;
    }

    // always transition
    public override Task<bool> EvaluateConditionsForTransition(IServiceProvider serviceProvider)
        => Task.FromResult(true);

    // execute on enter
    public override async Task OnEnter(IServiceProvider serviceProvider)
    {
        // query identified service
        var compartmentRepository = serviceProvider.GetRequiredService<CompartmentRepository>();
        var compartment = compartmentRepository.GetCompartment(_compartmentIdentifier.ToString());
        var service = compartment.Services.GetRequiredService<T>();
        
        // execute action on that service
        await _action(service);
    }
}
```

The adapter can now be utilized in Xcepto tests:

```csharp
await XceptoTest.Given(environment, builder =>
{
    var client = builder.RegisterAdapter(new HiveShardClientAdapter(credentials));
    Uri? connectedEdge = null;

    client.Action(x => x.Connect(credentials));
    client.Expect<ConnectionSucceeded>(x =>
    {
        connectedEdge = x.Edge;
        return true;
    });
    client.Action(x=> x.SendHotPathEvent(new EdgeBindingRequest(credentials)));
    client.Expect<EdgeBoundNotification>(x => x.Uri.Equals(connectedEdge));
});
```

All of this can be refined with fluentApis. The official `SSR` and `REST` adapters showcase this principle.

</details>

## Execution Models

Both async and enumerated execution models are supported.
The reason for async support is obvious for distributed systems.

Enumerated, on the other hand, supports execution flows in video games with game engines like *Unity*, as those allow Xcepto users to space execution apart over multiple frames. This also enables integrating waiting for gameplay events.

Here is an example `UnityTest`:
```csharp
[UnityTest]
public IEnumerator NonBlockingRuntimeTest()
{
    yield return XceptoTest.GivenEnumerated(new ExampleScenario(), builder =>
    {
        var ssr = builder.SsrAdapterBuilder()
            .WithBaseUrl(new Uri("https://xcepto.org"))
            .Build();
    
        ssr.Post("/auth/login")
            .WithFormContent(new LoginRequest(username, password).ToForm())
            .AssertSuccess();
    
        ssr.Get("/")
            .AssertThatResponseContentString(Does.Contain(username));
    });
}
```

## Compartments

Xcepto also boasts a full InMemory isolation system called **Compartments**. This enables hosting multiple systems inMemory that can communicate each other via explicitly defined interfaces. Compartmentalized services are still fully addressable in states via compartment identifiers.

```csharp
Compartment.From(new ServiceCollection()
    .AddSingleton<Service1>()
    .AddSingleton<PersonalDependency>() // individual instance
)
.ExposeService<Service1>() // shared instance
.Identify("service1")
.Build();

Compartment.From(new ServiceCollection()
    .AddSingleton<Service2>()
    .AddSingleton<PersonalDependency>() // individual instance
)
.DependsOn<Service1>() // consume shared instance
.Identify("service2")
.Build();

```

Identification in states works like this:

```csharp
public override async Task OnEnter(IServiceProvider serviceProvider)
{
    // query identified service
    var compartmentRepository = serviceProvider.GetRequiredService<CompartmentRepository>();
    var compartment = compartmentRepository.GetCompartment(_compartmentIdentifier.ToString());
    var service = compartment.Services.GetRequiredService<T>();
    
    // do something with service
}
```

## Resilience Guarantees

Xcepto is heavily tested in regards to resilience. It bolsters > 140 test cases, each targeting a set of options. Xcepto heavily employs matrix tests to cover every possibility.

Here is a summary of guaranteed properties:
- Cleanup execution **always** happens
- Compartments are truly **isolated**
- Async and Enumerated execution models cause **identical** results
- Exceptions always **short-circuit** failure and yield **specific error** details
- Concurrent non-blocking subsystem failures are **propagated** properly
- The supplied ILoggingProvider is **always** flushed, even on exception
- Transitions are **retried until timeout** according to specification
- Timeouts **always** happen! No blocking behaviour can stop this


Apart from that, correctness checks for official adapters are also in place.