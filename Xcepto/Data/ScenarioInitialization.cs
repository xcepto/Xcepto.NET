using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xcepto.Data;

public class ScenarioInitialization
{
    internal IEnumerable<Func<Task>> FireAndForgetTasks { get; }
    internal IEnumerable<Func<Task>> DoTasks { get; }

    internal ScenarioInitialization(IEnumerable<Func<Task>> doTasks, IEnumerable<Func<Task>> fireAndForgetTasks)
    {
        FireAndForgetTasks = fireAndForgetTasks;
        DoTasks = doTasks;
    }
}