using System.ComponentModel.DataAnnotations.Schema;

using Pgvector;

namespace BibleSemanticSearcher.Domain.Bible;
public sealed class Verse
{
    public required string Book { get; init; }
    public required string Text { get; init; }
    public required Uri Source { get; init; }
    public required int Chapter { get; init; }
    public required int VerseNumber { get; init; }
    [Column(TypeName = "vector(500)")]
    public Vector? Embedding { get; set; }
}
