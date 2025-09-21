# Xcepto.NET

[![Xcepto .NET release pipeline](https://github.com/xcepto/Xcepto.NET/actions/workflows/cd.yaml/badge.svg)](https://github.com/xcepto/Xcepto.NET/actions/workflows/cd.yaml)
[![semantic-release: conventional-commits](https://img.shields.io/badge/semantic--release-conventional--commits-e10079?logo=semantic-release)](https://www.conventionalcommits.org/en/v1.0.0/)
[![dotnet: netstandard2.0](https://img.shields.io/badge/compatibility-%E2%89%A5%20netstandard2.0-5632d5?logo=dotnet)](https://www.nuget.org/packages/Xcepto#supportedframeworks-body-tab)

Xcepto is a BDD testing framework for distributed systems.
Tests are specified **declaratively** here. 

## Getting Started

Test specification happens according to the [Given-When-Then](https://dannorth.net/blog/introducing-bdd/#atm-example) pattern.

### Mental model

Test steps are **not immediately executed** when called!
They are compiled into a **state machine**, 
where **transition** depends on the specified **conditions**.

The states are linked in a chain:

`[Start] -> [First] -> [Second] -> [Third] -> [Final]`

The test passes, if the `Final` state was reached before a **timeout**. 

### Given
`Xcepto.Given` introduces a specification environment based on a scenario.
```csharp
XceptoTest.Given(scenario, builder =>
{
    // Declare test behaviour here
}
```

`Scenario` classes specify instructions to setup and prepare the system under test.

### When (Actions)
Actions often require interfacing technologies.
Official adapters can be used to integrate some popular ones.

```csharp
XceptoTest.Given(scenario, builder =>
{
    var rest = builder.RegisterAdapter(new XceptoRestAdapter());

    // When
    rest.PostRequest<SomeRequest, SomeResponse>(
        "localhost:3000/", new SomeRequest(),  
        someReponse => someResponse.value > 1000);
}
```

Post requests in particular also enable response validation (so they are hybrid action/expectation).

### Then (Expectations) 

Expectations represent transition conditions.

RabbitMQ can be used to block transition until a certain kind of message is published.

```csharp
XceptoTest.Given(scenario, builder =>
{
    var rabbitMq = builder.RegisterAdapter(new XceptoRabbitMqAdapter(config));
    
    // When some Action happens
    
    // Then expect a certain response
    rabbitMq.EventCondition<ResponseMessage>(
        e => e.someValue == 1234);
}
```

### Full example

Expect that the backend message bus publishes `Search(Component)SearchedForArticle`
whenever the client initiates a Process with a `SearchForArticleRequest`.

```csharp
[Test]
public async Task Test()
{
    // Arrange
    var config = CustomRabbitMqConfig.GetRabbitMqConfig();
    var searchForArticleRequest = new SearchForArticleRequest
    {
        ArticleName = "Christmas Tree"
    };
    SearchForArticleRoute articleRoute = new SearchForArticleRoute();
    var postUrl = new Uri($"http://localhost:8080/{articleRoute.Path}");
    
    await XceptoTest.Given(new ChristmasGiftsScenario(), builder =>
    {
        var rest = builder.RegisterAdapter(new XceptoRestAdapter());
        var rabbitMq = builder.RegisterAdapter(new XceptoRabbitMqAdapter(config));
    
        // When
        rest.PostRequest<SearchForArticleRequest, SearchForArticleResponse>(
            postUrl, searchForArticleRequest,  _ => true);
        
        // Then
        rabbitMq.EventCondition<SearchSearchedForArticle>(
            e => e.ArticleName == searchForArticleRequest.ArticleName);
    });
}
```

Here, `ChristmasGiftsScenario` starts the production environment using **docker compose**.
The message bus also has to be configured, `GetRabbitMqConfig` returns a configuration 
that describes the exchanges, queues and keys and references the docker container as a host.

## Adapters

Xcepto supports several technologies through adapters.

**Core** library:
- Xcepto

Adapters:
- Xcepto.RabbitMQ (listening for messages)
- Xcepto.REST (sending POST requests, validating POST responses)
