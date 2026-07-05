namespace EnumeratedExecutionTests.Repositories;

public class Repository
{
    private int _number = 0;

    public void Increment()
    {
        _number++;
    }

    public int Get() => _number;
}