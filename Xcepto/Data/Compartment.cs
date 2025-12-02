using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Internal;

namespace Xcepto.Data;

public class Compartment
{
    public IServiceProvider Services { get; }
    private IEnumerable<ExposedService> _exposedServices;
    private DependencyProxy _dependencyProxy;
    public string UniqueName { get; }
    public Func<IServiceProvider, Task>? EntryPoint { get; } = null;

    internal Compartment(IServiceProvider serviceProvider, IEnumerable<ExposedService> exposedServices,
        DependencyProxy dependencyProxy, String uniqueName, Func<IServiceProvider, Task>? entryPoint)
    {
        UniqueName = uniqueName;
        EntryPoint = entryPoint;
        _dependencyProxy = dependencyProxy;
        _exposedServices = exposedServices;
        Services = serviceProvider;
    }
    public static CompartmentBuilder From(IServiceCollection serviceCollection)
    {
        return new CompartmentBuilder(serviceCollection);
    }
    
    internal void Activate(IServiceProvider externalProvider)
    {
        _dependencyProxy.Register(externalProvider);
    }
    
    internal IEnumerable<ExposedService> GetExposedServices()
    {
        return _exposedServices;
    }
}