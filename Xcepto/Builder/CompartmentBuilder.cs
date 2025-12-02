using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;

namespace Xcepto.Builder;

public class CompartmentBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private List<Type> _exposed = new();
    private DependencyProxy _dependencyProxy = new();
    private string _name = String.Empty;
    private Func<IServiceProvider, Task>? _entryPoint = null;

    internal CompartmentBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public Compartment Build()
    {
        if (_name.Equals(String.Empty))
            _name = Guid.NewGuid().ToString();
        
        var serviceProvider = _serviceCollection.BuildServiceProvider();
        List<ExposedService> exposedServices = new List<ExposedService>();
        foreach (var type in _exposed)
        {
            exposedServices.Add(new ExposedService(type, () => serviceProvider.GetRequiredService(type)));
        }
        return new Compartment(serviceProvider, exposedServices, _dependencyProxy, _name, _entryPoint);
    }

    public CompartmentBuilder SetEntryPoint<T>()
    where T: IEntryPoint
    {
        SetEntryPoint(typeof(T));
        return this;
    }
    
    public CompartmentBuilder SetEntryPoint(Type entryPointType)
    {
        _entryPoint = serviceProvider =>
        {
            var entryPointInstance = serviceProvider.GetRequiredService(entryPointType);
            if (entryPointInstance is not IEntryPoint entryPoint)
                throw new ArgumentException($"entry point type was not of type {nameof(IEntryPoint)}");
            return entryPoint.Start();
        };
        return this;
    }

    public CompartmentBuilder Identify(string uniqueName)
    {
        _name = uniqueName;
        return this;
    }

    public CompartmentBuilder ExposeService<TService>()
    {
        return ExposeService(typeof(TService));
    }

    public CompartmentBuilder ExposeService(Type type)
    {
        if (_serviceCollection.All(x => x.ServiceType != type))
            throw new ArgumentException($"No service of type {type} was contained in this compartment");
        _exposed.Add(type);
        return this;
    }
    
    public CompartmentBuilder DependsOn<TService>()
    where TService: class
    {
        return DependsOn(typeof(TService));
    }

    public CompartmentBuilder DependsOn(Type type)
    {
        _serviceCollection.AddSingleton(type, _ => _dependencyProxy.Get(type));
        return this;
    }
}