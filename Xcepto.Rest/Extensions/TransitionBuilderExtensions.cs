using Xcepto.Builder;
using Xcepto.Rest.Builders;

namespace Xcepto.Rest.Extensions;

public static class TransitionBuilderExtensions
{
    public static RestAdapterBuilder RestAdapterBuilder(this TransitionBuilder builder)
    {
        return new RestAdapterBuilder(builder);
    }
}