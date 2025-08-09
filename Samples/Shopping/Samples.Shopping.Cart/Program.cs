using MassTransit;
using Samples.Shopping.Cart;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    var rabbitmqEndpoint = builder.Configuration["RABBITMQ_URL"] 
                           ?? Environment.GetEnvironmentVariable("RABBITMQ_URL");

    x.AddConsumer<ClientAddedSelectedArticleToCartConsumer>();
    
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

var host = builder.Build();
host.Run();