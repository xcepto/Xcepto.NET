namespace Compartments.Tests.Dependencies;

public class SharedDependency
{
    private int _number = 0;
    public void Increment()
    {
        _number++;
    }

    public int Value() => _number;
}