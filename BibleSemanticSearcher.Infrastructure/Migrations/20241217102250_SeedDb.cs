using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleSemanticSearcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string url = "https://raw.githubusercontent.com/thiagobodruk/bible/refs/heads/master/json/es_rvr.json";

            using (var client = new HttpClient())
            {
                var json = client.GetStringAsync(url).GetAwaiter().GetResult();
                var books = JsonSerializer.Deserialize<List<Book>>(json);

                if (books != null)
                {
                    foreach (var book in books)
                    {
                        for (int chapterIndex = 0; chapterIndex < book.Chapters.Count; chapterIndex++)
                        {
                            var chapter = book.Chapters[chapterIndex];
                            for (int verseIndex = 0; verseIndex < chapter.Count; verseIndex++)
                            {
                                var verseText = chapter[verseIndex];

                                migrationBuilder.Sql($@"
                                    INSERT INTO verses (""Book"", ""Chapter"", ""VerseNumber"", ""Text"", ""Source"")
                                    VALUES (
                                        '{book.Name.Replace("'", "''")}', 
                                        {chapterIndex + 1}, 
                                        {verseIndex + 1}, 
                                        '{verseText.Replace("'", "''")}', 
                                        '{url}'
                                    )
                                ");
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove all seeded verses
            migrationBuilder.Sql("DELETE FROM verses");
        }
    }

    public class Book
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
        [JsonPropertyName("chapters")]
        public required List<List<string>> Chapters { get; init; }
    }
}
