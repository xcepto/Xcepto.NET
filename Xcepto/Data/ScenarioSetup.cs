using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto.Data;

public class ScenarioSetup
{
    public IEnumerable<Type> Disposables { get; }
    public IEnumerable<Func<Task>> DoTasks { get; }
    internal IServiceCollection ServiceCollection { get; }

    internal ScenarioSetup(IEnumerable<Func<Task>> doTasks, IServiceCollection serviceCollection,
        IEnumerable<Type> asEnumerable)
    {
        Disposables = asEnumerable;
        DoTasks = doTasks;
        ServiceCollection = serviceCollection;
    }
}