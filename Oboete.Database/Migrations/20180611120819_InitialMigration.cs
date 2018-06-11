using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Oboete.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    CreateDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentName = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardType",
                columns: table => new
                {
                    FlashcardTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlashcardTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardType", x => x.FlashcardTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NoteType",
                columns: table => new
                {
                    NoteTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.NoteTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    CreateDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Kana = table.Column<string>(nullable: true),
                    Kanji = table.Column<string>(nullable: true),
                    ExampleSentenceJapanese = table.Column<string>(nullable: true),
                    EnglishMeaning = table.Column<string>(nullable: true),
                    ExampleSentenceEnglish = table.Column<string>(nullable: true),
                    WordAudioDocumentId = table.Column<int>(nullable: false),
                    NoteTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_NoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalTable: "NoteType",
                        principalColumn: "NoteTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Note_Document_WordAudioDocumentId",
                        column: x => x.WordAudioDocumentId,
                        principalTable: "Document",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flashcard",
                columns: table => new
                {
                    CreateDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    FlashCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteId = table.Column<int>(nullable: false),
                    FlashcardTypeId = table.Column<int>(nullable: false),
                    EaseFactor = table.Column<double>(nullable: false),
                    IntervalIndex = table.Column<int>(nullable: false),
                    NextReviewDateTime = table.Column<DateTimeOffset>(nullable: false),
                    LastReviewDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.FlashCardId);
                    table.ForeignKey(
                        name: "FK_Flashcard_FlashcardType_FlashcardTypeId",
                        column: x => x.FlashcardTypeId,
                        principalTable: "FlashcardType",
                        principalColumn: "FlashcardTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flashcard_Note_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Note",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Document",
                columns: new[] { "DocumentId", "CreateDateTime", "DocumentName", "FileName", "ModifiedDateTime" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2018, 6, 11, 12, 8, 18, 671, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("f0c4c8f8-c080-4d31-b7e1-527491f87b0f"), "TestDocument.text", new DateTimeOffset(new DateTime(2018, 6, 11, 12, 8, 18, 671, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "FlashcardType",
                columns: new[] { "FlashcardTypeId", "FlashcardTypeName" },
                values: new object[,]
                {
                    { 1, "Kana Recognition" },
                    { 2, "Kana Production" }
                });

            migrationBuilder.InsertData(
                table: "NoteType",
                columns: new[] { "NoteTypeId", "NoteTypeName" },
                values: new object[,]
                {
                    { 1, "Vocab" },
                    { 2, "Grammar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_FlashcardTypeId",
                table: "Flashcard",
                column: "FlashcardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_NoteId",
                table: "Flashcard",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NoteTypeId",
                table: "Note",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_WordAudioDocumentId",
                table: "Note",
                column: "WordAudioDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "FlashcardType");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "NoteType");

            migrationBuilder.DropTable(
                name: "Document");
        }
    }
}
