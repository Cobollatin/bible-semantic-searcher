using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibleSemanticSearcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "verses",
                columns: table => new
                {
                    Book = table.Column<string>(type: "varchar(50)", nullable: false),
                    Chapter = table.Column<int>(type: "int", nullable: false),
                    VerseNumber = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verses", x => new { x.Book, x.Chapter, x.VerseNumber });
                });

            migrationBuilder.CreateIndex(
                name: "IX_verses_Text",
                table: "verses",
                column: "Text")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "spanish");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verses");
        }
    }
}
