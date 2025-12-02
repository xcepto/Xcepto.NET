using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto.Data;

public class ScenarioSetup
{
    public IEnumerable<Func<Task>> DoTasks { get; }
    internal IServiceCollection ServiceCollection { get; }

    internal ScenarioSetup(IEnumerable<Func<Task>> doTasks, IServiceCollection serviceCollection)
    {
        DoTasks = doTasks;
        ServiceCollection = serviceCollection;
    }
}