using Microsoft.EntityFrameworkCore.Migrations;

using Pgvector;

#nullable disable

namespace BibleSemanticSearcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmbeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS vector;");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.AddColumn<Vector>(
                name: "Embedding",
                table: "verses",
                type: "vector(500)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_verses_Embedding",
                table: "verses",
                column: "Embedding")
                .Annotation("Npgsql:IndexMethod", "hnsw")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_l2_ops" })
                .Annotation("Npgsql:StorageParameter:ef_construction", 64)
                .Annotation("Npgsql:StorageParameter:m", 16);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_verses_Embedding",
                table: "verses");

            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "verses");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:vector", ",,");
        }
    }
}
