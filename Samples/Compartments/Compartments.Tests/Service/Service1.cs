using Compartments.Tests.Dependencies;

namespace Compartments.Tests.Service;

public class Service1
{
    private Dependency1 _dependency1;

    public Service1(Dependency1 dependency1)
    {
        _dependency1 = dependency1;
    }

    public void Act()
    {
        _dependency1.Increment();
    }

    public int GetValue()
    {
        return _dependency1.Value();
    }
}