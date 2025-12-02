using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xcepto.Data;

namespace Xcepto.Builder;

public class ScenarioSetupBuilder
{
    internal ScenarioSetupBuilder() { }
    private readonly List<Func<Task>> _tasks = new();
    private IServiceCollection _serviceCollection = new ServiceCollection();
    public ScenarioSetup Build()
    {
        return new ScenarioSetup(_tasks.AsEnumerable(), _serviceCollection);
    }

    public ScenarioSetupBuilder AddServices(Func<IServiceCollection, IServiceCollection> services)
    {
        _serviceCollection = services(_serviceCollection);
        return this;
    }
    
    public ScenarioSetupBuilder Do(Func<Task> task)
    {
        _tasks.Add(task);
        return this;
    }
    
    public ScenarioSetupBuilder Do(Action task)
    {
        _tasks.Add(() =>
        {
            task();
            return Task.CompletedTask;
        });
        return this;
    }
}