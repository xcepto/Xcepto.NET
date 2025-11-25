using Compartments.Tests.Repositories;
using Xcepto.Interfaces;

namespace Compartments.Tests.Service;

public class WorkerService: IEntryPoint
{
    private TaskRepository _taskRepository;
    private ResultRepository _resultRepository;

    public WorkerService(TaskRepository taskRepository, ResultRepository resultRepository)
    {
        _taskRepository = taskRepository;
        _resultRepository = resultRepository;
    }

    public async Task Start()
    {
        while (true)
        {
            while (_taskRepository.TryDequeue(out Guid task))
            {
                _resultRepository.Add(task);
            }

            await Task.Delay(100);
        }
    }
}