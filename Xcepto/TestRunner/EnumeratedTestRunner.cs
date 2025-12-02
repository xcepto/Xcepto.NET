using System.Collections;
using System.Collections.Generic;

namespace Xcepto.TestRunner;

public class EnumeratedTestRunner
{
    public static void RunEnumerator(IEnumerator routine)
    {
        Stack<IEnumerator> stack = new();
        stack.Push(routine);

        while (true)
        {
            if (stack.Count == 0)
                return;

            var top = stack.Peek();

            bool active = top.MoveNext();

            if (!active)
            {
                stack.Pop();
                continue;
            }

            var yielded = top.Current;

            if (yielded is IEnumerator nested)
                stack.Push(nested);
        }
    }


}