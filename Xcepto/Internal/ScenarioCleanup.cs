using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xcepto.Internal;

public class ScenarioCleanup
{
    internal IEnumerable<Func<Task>> DoTasks { get; }

    internal ScenarioCleanup(IEnumerable<Func<Task>> doTasks)
    {
        DoTasks = doTasks;
    }
}