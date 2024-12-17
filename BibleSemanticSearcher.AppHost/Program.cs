using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
        .WithDataVolume(isReadOnly: false)
        .WithPgAdmin();

var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.BibleSemanticSearcher_ApiService>("backend")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddNpmApp("frontend", "../BibleSemanticSearcher.Web")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

var app = builder.Build();
await app.RunAsync(CancellationToken.None);