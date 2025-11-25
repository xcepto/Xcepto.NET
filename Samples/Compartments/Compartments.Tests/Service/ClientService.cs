using Compartments.Tests.Repositories;

namespace Compartments.Tests.Service;

public class ClientService
{
    private ResultRepository _resultRepository;
    private TaskRepository _taskRepository;

    public ClientService(ResultRepository resultRepository, TaskRepository taskRepository)
    {
        _resultRepository = resultRepository;
        _taskRepository = taskRepository;
    }

    public void CreateTask(Guid task)
    {
        _taskRepository.Enqueue(task);
    }


    public bool CheckCompletion(Guid firstGuid)
    {
        return _resultRepository.TryGet(firstGuid);
    }
}