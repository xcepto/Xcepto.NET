namespace Compartments.Tests.Dependencies;

public class Dependency1
{
    private int _number = 0;
    public void Increment()
    {
        _number++;
    }

    public int Value() => _number;
}