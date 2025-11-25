using System;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Repositories;

namespace Xcepto.Util;

public static class ServiceProviderExtensions
{
    public static T GetCompartmentalizedService<T>(this IServiceProvider serviceProvider, string compartmentIdentifier)
    where T: notnull
    {
        var compartmentRepository = serviceProvider.GetRequiredService<CompartmentRepository>();
        var compartment = compartmentRepository.GetCompartment(compartmentIdentifier);
        return compartment.Services.GetRequiredService<T>();
    }
}