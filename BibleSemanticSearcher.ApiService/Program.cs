using BibleSemanticSearcher.Application;
using BibleSemanticSearcher.Domain.Bible;
using BibleSemanticSearcher.Infrastructure;
using BibleSemanticSearcher.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddCors();

builder.Logging.AddConsole();

builder.AddApplication();
builder.AddInfrastructure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BibleDbContext>();
    if ((await context.Database.GetPendingMigrationsAsync()).Any())
    {
        await context.Database.MigrateAsync();
    }
}


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/api/bible", (string search, BibleDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Received search request with term: {SearchTerm}", search);
    var verses = context.Verses
        .Where(b =>
            EF.Functions.ToTsVector("spanish", b.Text)
                .Matches(EF.Functions.PhraseToTsQuery("spanish", search)))
        .ToList();
    return verses;
})
.WithName("SearchBibleVerses");

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.MapDefaultEndpoints();

await app.RunAsync(CancellationToken.None);

