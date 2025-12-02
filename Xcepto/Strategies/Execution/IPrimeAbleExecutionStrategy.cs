using Xcepto.Internal;

namespace Xcepto.Strategies.Execution;

internal interface IPrimeAbleExecutionStrategy: IExecutionStrategy
{
    internal void Prime(TestInstance testInstance);
}