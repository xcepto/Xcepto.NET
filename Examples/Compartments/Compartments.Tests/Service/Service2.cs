using Compartments.Tests.Dependencies;

namespace Compartments.Tests.Service;

public class Service2
{
    private PersonalDependency _personalDependency;
    private SharedDependency _sharedDependency;

    public Service2(PersonalDependency personalDependency, SharedDependency sharedDependency)
    {
        _personalDependency = personalDependency;
        _sharedDependency = sharedDependency;
    }

    public void Act()
    {
        _personalDependency.Increment();
        _sharedDependency.Increment();
    }

    public int GetValue() => _personalDependency.Value();
}