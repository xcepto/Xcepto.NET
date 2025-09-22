using System;
using System.Collections.Generic;

namespace Xcepto.Data;

internal class DependencyProxy
{
    private IServiceProvider? _serviceProvider;

    internal TService Get<TService>()
    {
        if (_serviceProvider is null)
            throw new Exception("Compartment not activated yet");
        var type = typeof(TService);
        var service = _serviceProvider.GetService(type);
        if (service is null)
            throw new Exception($"External dependency of type {type.Name} not found");
        return (TService)service;
    }

    internal void Register(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}