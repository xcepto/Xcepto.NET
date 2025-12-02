using System;

namespace Xcepto.Internal;

internal class ExposedService
{
    public ExposedService(Type type, Func<object> instanceSupplier)
    {
        Type = type;
        InstanceSupplier = instanceSupplier;
    }

    public Type Type { get; }
    public Func<object> InstanceSupplier { get; }
}