namespace Compartments.Tests.Dependencies;

public class PersonalDependency
{
    private int _number = 0;
    public void Increment()
    {
        _number++;
    }

    public int Value() => _number;
}