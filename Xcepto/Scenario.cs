using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Xcepto
{
    public abstract class Scenario
    {
        private TransitionBuilder? _builder;

        protected void Propagate(Task task)
        {
            _builder.PropagateExeptions(task);
        }
        internal void AssignBuilder(TransitionBuilder builder)
        {
            _builder = builder;
        }
        public abstract Task<IServiceCollection> Setup();

        public abstract Task Initialize(IServiceProvider serviceProvider);

        public abstract Task Cleanup(IServiceProvider serviceProvider);
    }
}
