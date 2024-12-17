using BibleSemanticSearcher.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BibleSemanticSearcher.Infrastructure;

public static class Configure
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<BibleDbContext>(connectionName: "postgresdb", configureDbContextOptions: (options) =>
        {
            options.UseNpgsql(o => o.UseVector());
        });
        return builder;
    }
}
