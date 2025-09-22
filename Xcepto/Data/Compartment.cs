using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;

namespace Xcepto.Data;

public class Compartment
{
    private IServiceProvider _serviceProvider;
    private IEnumerable<ExposedService> _exposedServices;
    private DependencyProxy _dependencyProxy;

    internal Compartment(IServiceProvider serviceProvider, IEnumerable<ExposedService> exposedServices,
        DependencyProxy dependencyProxy)
    {
        _dependencyProxy = dependencyProxy;
        _exposedServices = exposedServices;
        _serviceProvider = serviceProvider;
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