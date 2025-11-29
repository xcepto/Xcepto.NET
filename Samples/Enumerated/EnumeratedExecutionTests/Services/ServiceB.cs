using EnumeratedExecutionTests.Repositories;

namespace EnumeratedExecutionTests.Services;

public class ServiceB
{
    private Repository _repository;

    public ServiceB(Repository repository)
    {
        _repository = repository;
    }

    public void Act()
    {
        _repository.Increment();
    }
}