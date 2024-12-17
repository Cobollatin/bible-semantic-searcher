using BibleSemanticSearcher.Infrastructure.Persistence;
using Microsoft.Extensions.Hosting;

namespace BibleSemanticSearcher.Infrastructure;

public static class Configure
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<BibleDbContext>(connectionName: "postgresdb");
        return builder;
    }
}
