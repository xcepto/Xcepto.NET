using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class XceptoScenario
    {
        private TransitionBuilder? _builder;

        protected void PropagateExceptions(Task task)
        {
            _builder!.PropagateExceptions(task);
        }
        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }

        public virtual Task<IServiceCollection> Setup() => Task.FromResult<IServiceCollection>(new ServiceCollection());

        public virtual Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

        public virtual Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
    }
}
