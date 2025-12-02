using System;

namespace Xcepto.Internal;

internal class ExposedService
{
    internal ExposedService(Type type, Func<object> instanceSupplier)
    {
        Type = type;
        InstanceSupplier = instanceSupplier;
    }

    internal Type Type { get; }
    internal Func<object> InstanceSupplier { get; }
}