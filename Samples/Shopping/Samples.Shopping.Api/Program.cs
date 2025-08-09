using MassTransit;
using Samples.Shopping.Api.Contracts.routes;
using Samples.Shopping.Events.telemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(x =>
{
    var rabbitmqEndpoint = builder.Configuration["RABBITMQ_URL"] 
                       ?? Environment.GetEnvironmentVariable("RABBITMQ_URL");
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitmqEndpoint, "/", credentials =>
        {
            credentials.Username("guest");
            credentials.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

// builder.Services.AddControllers(); use minimal api instead
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var goToHomepageRoute = new GoToHomepageRoute();
app.MapPost(goToHomepageRoute.Path, (GoToHomepageRequest request, IBus bus) =>
{
    bus.Publish<ClientVisitedHomepageEvent>(new ClientVisitedHomepageEvent());
    return new GoToHomepageResponse()
    {
        Trace = request.Trace
    };
})
.WithName(goToHomepageRoute.Path);

var searchForArticleRoute = new SearchForArticleRoute();
app.MapPost(searchForArticleRoute.Path, (SearchForArticleRequest request, IBus bus) =>
{
    bus.Publish<ClientSearchedForArticle>(new ClientSearchedForArticle()
    {
        Name = request.ArticleName
    });
    return new SearchForArticleResponse()
    {
        Message = "Articles are being searched"
    };
})
.WithName(searchForArticleRoute.Path);

var addSelectedArticleToCartRoute = new AddSelectedArticleToCartRoute();
app.MapPost(addSelectedArticleToCartRoute.Path, (AddSelectedArticleToCartRequest request, IBus bus) =>
{
    bus.Publish<ClientAddedSelectedArticleToCart>(new ClientAddedSelectedArticleToCart()
    {
        ArticleName = request.ArticleName
    });

    return new AddSelectedArticleToCartResponse()
    {
        ArticleName = request.ArticleName
    };
})
.WithName(addSelectedArticleToCartRoute.Path);

app.Run();