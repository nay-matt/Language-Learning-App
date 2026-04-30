using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Language_Learning_App.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsToFlashcards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardTags",
                columns: table => new
                {
                    FlashcardsFlashcardID = table.Column<int>(type: "int", nullable: false),
                    TagsTagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardTags", x => new { x.FlashcardsFlashcardID, x.TagsTagID });
                    table.ForeignKey(
                        name: "FK_FlashcardTags_Flashcards_FlashcardsFlashcardID",
                        column: x => x.FlashcardsFlashcardID,
                        principalTable: "Flashcards",
                        principalColumn: "FlashcardID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardTags_Tags_TagsTagID",
                        column: x => x.TagsTagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardTags_TagsTagID",
                table: "FlashcardTags",
                column: "TagsTagID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashcardTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
