using EnumeratedExecutionTests.Repositories;

namespace EnumeratedExecutionTests.Services;

public class ServiceA
{
    private Repository _repository;

    public ServiceA(Repository repository)
    {
        _repository = repository;
    }

    public void Act()
    {
        _repository.Increment();
    }
}