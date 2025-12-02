using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcepto.Internal;

namespace Xcepto.Builder;

public class ScenarioInitializationBuilder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<Func<Task>> _tasks = new();
    private readonly List<Func<Task>> _fireAndForgetFunctions = new();
    public ScenarioInitializationBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ScenarioInitialization Build()
    {
        return new ScenarioInitialization(_tasks.AsEnumerable(), _fireAndForgetFunctions.AsEnumerable());
    }
    
    public ScenarioInitializationBuilder Do(Func<Task> task)
    {
        _tasks.Add(task);
        return this;
    }
    
    public ScenarioInitializationBuilder Do(Func<IServiceProvider, Task> task)
    {
        _tasks.Add(() => task(_serviceProvider));
        return this;
    }
    
    public ScenarioInitializationBuilder Do(Action task)
    {
        _tasks.Add(() =>
        {
            task();
            return Task.CompletedTask;
        });
        return this;
    }
    
    public ScenarioInitializationBuilder Do(Action<IServiceProvider> task)
    {
        _tasks.Add(() =>
        {
            task(_serviceProvider);
            return Task.CompletedTask;
        });
        return this;
    }

    public ScenarioInitializationBuilder FireAndForget(Func<IServiceProvider, Task> task)
    {
        _fireAndForgetFunctions.Add(() => task(_serviceProvider));
        return this;
    }
}