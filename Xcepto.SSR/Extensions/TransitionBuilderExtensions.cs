using Xcepto.Builder;
using Xcepto.SSR.Builders;

namespace Xcepto.SSR.Extensions;

public static class TransitionBuilderExtensions
{
    public static SsrAdapterBuilder SsrAdapterBuilder(this TransitionBuilder builder)
    {
        return new SsrAdapterBuilder(builder);
    }
}