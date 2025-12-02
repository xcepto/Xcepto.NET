using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xcepto.Internal;

public class ScenarioInitialization
{
    public IEnumerable<Func<Task>> FireAndForgetTasks { get; }
    public IEnumerable<Func<Task>> DoTasks { get; }

    public ScenarioInitialization(IEnumerable<Func<Task>> doTasks, IEnumerable<Func<Task>> fireAndForgetTasks)
    {
        FireAndForgetTasks = fireAndForgetTasks;
        DoTasks = doTasks;
    }
}