using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Data;

namespace Xcepto.Builder;

public class CompartmentBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private List<Type> _exposed = new();
    private DependencyProxy _dependencyProxy = new();

    internal CompartmentBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public Compartment Build()
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();
        List<ExposedService> exposedServices = new List<ExposedService>();
        foreach (var type in _exposed)
        {
            exposedServices.Add(new ExposedService(type, () => serviceProvider.GetRequiredService(type)));
        }
        return new Compartment(serviceProvider, exposedServices, _dependencyProxy);
    }

    public CompartmentBuilder ExposeService<TService>()
    {
        if (_serviceCollection.All(x => x.ServiceType != typeof(TService)))
            throw new ArgumentException($"No service of type {typeof(TService)} was contained in this compartment");
        _exposed.Add(typeof(TService));
        return this;
    }

    public CompartmentBuilder DependsOn<TService>()
    where TService: class
    {
        _serviceCollection.AddSingleton<TService>(_ => _dependencyProxy.Get<TService>());
        return this;
    }
}