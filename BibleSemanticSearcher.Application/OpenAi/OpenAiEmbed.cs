using OpenAI;
using OpenAI.Embeddings;

namespace BibleSemanticSearcher.Application.OpenAi;
public static class OpenAiEmbed
{
    private const string Model = "text-embedding-3-small";
    private const string ApiKey = "<>";

    public static async Task<float[]> EmbedAsync(string text, int dimensions, CancellationToken cancellationToken)
    {
        var api = new OpenAIClient(ApiKey);
        var client = api.GetEmbeddingClient(Model);
        var options = new EmbeddingGenerationOptions()
        {
            Dimensions = dimensions,
        };
        OpenAIEmbedding response = await client.GenerateEmbeddingAsync(text, options, cancellationToken);
        return response.ToFloats().ToArray();
    }
}
