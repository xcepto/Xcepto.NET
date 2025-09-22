using System;

namespace Xcepto.Data;

internal class ExposedService
{
    public ExposedService(Type type, object instance)
    {
        Type = type;
        Instance = instance;
    }

    public Type Type { get; }
    public object Instance { get; }
}