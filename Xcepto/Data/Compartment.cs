using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;

namespace Xcepto.Data;

public class Compartment
{
    private IServiceProvider _serviceProvider;
    private IEnumerable<ExposedService> _exposedServices;

    internal Compartment(IServiceProvider serviceProvider, IEnumerable<ExposedService> exposedServices)
    {
        _exposedServices = exposedServices;
        _serviceProvider = serviceProvider;
    }
    public static CompartmentBuilder From(IServiceCollection serviceCollection)
    {
        return new CompartmentBuilder(serviceCollection);
    }
    
    internal IEnumerable<ExposedService> GetExposedServices()
    {
        return _exposedServices;
    }
}