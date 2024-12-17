using BibleSemanticSearcher.Application;
using BibleSemanticSearcher.Application.OpenAi;
using BibleSemanticSearcher.Infrastructure;
using BibleSemanticSearcher.Infrastructure.Persistence;
using Pgvector;

using Microsoft.EntityFrameworkCore;

using Pgvector.EntityFrameworkCore;

using System.Numerics;
using Vector = Pgvector.Vector;

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

    var verses = await context.Verses
        .Where(verse => verse.Embedding == null)
        .ToListAsync();

    const int maxConcurrency = 5;
    var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

    var verseChunks = verses
        .Select((v, i) => new { Verse = v, Index = i })
        .GroupBy(x => x.Index / 100)
        .Select(g => g.Select(x => x.Verse).ToList())
        .ToList();

    foreach (var chunk in verseChunks)
    {
        var tasks = chunk.Select(async verse =>
        {
            await semaphore.WaitAsync();
            try
            {
                var floats = await OpenAiEmbed.EmbedAsync(verse.Text, 500, CancellationToken.None);
                verse.Embedding = new Vector(floats);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
        context.Verses.UpdateRange(chunk);
        await context.SaveChangesAsync();
    }

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Embeddings generated for {Count} verses", verses.Count);

    logger.LogInformation("Application is ready.");
}


// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/api/bible", async (string search, BibleDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Received search request with term: {SearchTerm}", search);


    //var verses = await context.Verses
    //    .Where(b =>
    //        EF.Functions.ToTsVector("spanish", b.Text)
    //            .Matches(EF.Functions.PhraseToTsQuery("spanish", search)))
    //    .ToListAsync();

    var floats = await OpenAiEmbed.EmbedAsync(search, 500, CancellationToken.None);
    var vector = new Vector(floats);
    logger.LogInformation("Embedding generated: {Embedding}", vector);

    var distance = 1.0e-0;

    var verses = await context.Verses
        .Where(b => Math.Abs(b.Embedding!.CosineDistance(vector)) < distance)
        .OrderBy(b => Math.Abs(b.Embedding!.CosineDistance(vector)))
        .Select(x => new { Entity = x, Distance = x.Embedding!.L2Distance(vector) })
        .Take(9)
        .ToListAsync(CancellationToken.None);

    // We log the distances
    foreach (var verse in verses)
    {
        logger.LogInformation("Verse: {VerseText}, Distance: {Distance}", verse.Entity.Text, verse.Distance);
    }

    return verses
    .Select(verse => verse.Entity)
    .ToList();
})
.WithName("SearchBibleVerses");

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.MapDefaultEndpoints();

await app.RunAsync(CancellationToken.None);

