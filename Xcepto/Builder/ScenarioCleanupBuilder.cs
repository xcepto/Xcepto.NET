using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcepto.Internal;

namespace Xcepto.Builder;

public class ScenarioCleanupBuilder
{
    private readonly List<Func<Task>> _doTasks = new();
    private readonly IServiceProvider _serviceProvider;
    internal ScenarioCleanupBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ScenarioCleanup Build()
    {
        return new ScenarioCleanup(_doTasks.AsEnumerable());
    }

    public ScenarioCleanupBuilder Do(Func<Task> task)
    {
        _doTasks.Add(task);
        return this;
    }
    
    public ScenarioCleanupBuilder Do(Func<IServiceProvider, Task> task)
    {
        _doTasks.Add(() => task(_serviceProvider));
        return this;
    }
    
    public ScenarioCleanupBuilder Do(Action task)
    {
        _doTasks.Add(() =>
        {
            task();
            return Task.CompletedTask;
        });
        return this;
    }
    
    public ScenarioCleanupBuilder Do(Action<IServiceProvider> task)
    {
        _doTasks.Add(() =>
        {
            task(_serviceProvider);
            return Task.CompletedTask;
        });
        return this;
    }
}