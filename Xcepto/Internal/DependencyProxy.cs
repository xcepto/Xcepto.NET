using System;

namespace Xcepto.Internal;

internal class DependencyProxy
{
    private IServiceProvider? _serviceProvider;

    internal TService Get<TService>()
    {
        return (TService)Get(typeof(TService));
    }
    
    internal object Get(Type type)
    {
        if (_serviceProvider is null)
            throw new Exception("Compartment not activated yet");
        var service = _serviceProvider.GetService(type);
        if (service is null)
            throw new Exception($"External dependency of type {type.Name} not found");
        return service;
    }

    internal void Register(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}