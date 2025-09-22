using Compartments.Tests.Dependencies;

namespace Compartments.Tests.Service;

public class Service1
{
    private PersonalDependency _personalDependency;
    private SharedDependency _sharedDependency;

    public Service1(PersonalDependency personalDependency, SharedDependency sharedDependency)
    {
        _personalDependency = personalDependency;
        _sharedDependency = sharedDependency;
    }

    public void Act()
    {
        _personalDependency.Increment();
        _sharedDependency.Increment();
    }

    public int GetValue()
    {
        return _personalDependency.Value();
    }
}