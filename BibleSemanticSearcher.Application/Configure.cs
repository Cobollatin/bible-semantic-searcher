using Microsoft.Extensions.Hosting;

namespace BibleSemanticSearcher.Application;

public static class Configure
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
