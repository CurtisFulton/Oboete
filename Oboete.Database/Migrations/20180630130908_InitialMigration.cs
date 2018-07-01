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
                name: "OboeteUser",
                columns: table => new
                {
                    OboeteUserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    LoginFailedCount = table.Column<int>(nullable: false),
                    LockoutEndDateTime = table.Column<DateTimeOffset>(nullable: true),
                    SecurityStamp = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OboeteUser", x => x.OboeteUserID);
                });

            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    DeckID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeckName = table.Column<string>(nullable: false),
                    OboeteUserID = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.DeckID);
                    table.ForeignKey(
                        name: "FK_Deck_OboeteUser_OboeteUserID",
                        column: x => x.OboeteUserID,
                        principalTable: "OboeteUser",
                        principalColumn: "OboeteUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteType",
                columns: table => new
                {
                    NoteTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteTypeName = table.Column<string>(nullable: false),
                    OboeteUserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.NoteTypeID);
                    table.ForeignKey(
                        name: "FK_NoteType_OboeteUser_OboeteUserID",
                        column: x => x.OboeteUserID,
                        principalTable: "OboeteUser",
                        principalColumn: "OboeteUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OboeteUserLoginToken",
                columns: table => new
                {
                    OboeteUserLoginTokenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OboeteUserID = table.Column<int>(nullable: false),
                    LoginSelector = table.Column<Guid>(nullable: false),
                    TokenHash = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTimeOffset>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OboeteUserLoginToken", x => x.OboeteUserLoginTokenID);
                    table.ForeignKey(
                        name: "FK_OboeteUserLoginToken_OboeteUser_OboeteUserID",
                        column: x => x.OboeteUserID,
                        principalTable: "OboeteUser",
                        principalColumn: "OboeteUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardTemplate",
                columns: table => new
                {
                    FlashcardTemplateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteTypeID = table.Column<int>(nullable: false),
                    FlashcardTemplateName = table.Column<string>(nullable: false),
                    CardFrontTemplate = table.Column<string>(nullable: true),
                    CardBackTemplate = table.Column<string>(nullable: true),
                    CardStyling = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardTemplate", x => x.FlashcardTemplateID);
                    table.ForeignKey(
                        name: "FK_FlashcardTemplate_NoteType_NoteTypeID",
                        column: x => x.NoteTypeID,
                        principalTable: "NoteType",
                        principalColumn: "NoteTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeckID = table.Column<int>(nullable: false),
                    NoteTypeID = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteID);
                    table.ForeignKey(
                        name: "FK_Note_Deck_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Deck",
                        principalColumn: "DeckID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Note_NoteType_NoteTypeID",
                        column: x => x.NoteTypeID,
                        principalTable: "NoteType",
                        principalColumn: "NoteTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteTypeFieldDefinition",
                columns: table => new
                {
                    NoteTypeFieldDefinitionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteTypeID = table.Column<int>(nullable: false),
                    NoteFieldName = table.Column<string>(nullable: false),
                    NoteFieldDisplay = table.Column<string>(nullable: false),
                    SequenceID = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypeFieldDefinition", x => x.NoteTypeFieldDefinitionID);
                    table.ForeignKey(
                        name: "FK_NoteTypeFieldDefinition_NoteType_NoteTypeID",
                        column: x => x.NoteTypeID,
                        principalTable: "NoteType",
                        principalColumn: "NoteTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flashcard",
                columns: table => new
                {
                    FlashCardID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlashcardTemplateID = table.Column<int>(nullable: false),
                    NoteID = table.Column<int>(nullable: false),
                    EaseFactor = table.Column<double>(nullable: false),
                    IntervalIndex = table.Column<int>(nullable: false),
                    NextReviewDateTime = table.Column<DateTimeOffset>(nullable: false),
                    LastReviewDateTime = table.Column<DateTimeOffset>(nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.FlashCardID);
                    table.ForeignKey(
                        name: "FK_Flashcard_FlashcardTemplate_FlashcardTemplateID",
                        column: x => x.FlashcardTemplateID,
                        principalTable: "FlashcardTemplate",
                        principalColumn: "FlashcardTemplateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flashcard_Note_NoteID",
                        column: x => x.NoteID,
                        principalTable: "Note",
                        principalColumn: "NoteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteFieldValue",
                columns: table => new
                {
                    NoteFieldValueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NoteTypeFieldDefinitionId = table.Column<int>(nullable: false),
                    NoteId = table.Column<int>(nullable: false),
                    NoteValue = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedDateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteFieldValue", x => x.NoteFieldValueId);
                    table.ForeignKey(
                        name: "FK_NoteFieldValue_Note_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Note",
                        principalColumn: "NoteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteFieldValue_NoteTypeFieldDefinition_NoteTypeFieldDefinitionId",
                        column: x => x.NoteTypeFieldDefinitionId,
                        principalTable: "NoteTypeFieldDefinition",
                        principalColumn: "NoteTypeFieldDefinitionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OboeteUser",
                columns: new[] { "OboeteUserID", "CreatedDateTime", "Email", "IsEmailConfirmed", "LockoutEndDateTime", "LoginFailedCount", "ModifiedDateTime", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 286, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Curtis.R.Fulton@Gmail.com", false, null, 0, new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 286, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "$2y$12$nu0WVC4LHuf1kbAKTNQQ8ueIQmW5dWEXhatBPz5xvPvVzpJx6Qmm2", new Guid("f98a09ce-ff28-4d21-853b-a9dc83f2f7c8"), "Temp User" });

            migrationBuilder.InsertData(
                table: "Deck",
                columns: new[] { "DeckID", "CreatedDateTime", "DeckName", "ModifiedDateTime", "OboeteUserID" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 288, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Default Deck", new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 288, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "NoteType",
                columns: new[] { "NoteTypeID", "NoteTypeName", "OboeteUserID" },
                values: new object[] { 1, "Default Note", 1 });

            migrationBuilder.InsertData(
                table: "FlashcardTemplate",
                columns: new[] { "FlashcardTemplateID", "CardBackTemplate", "CardFrontTemplate", "CardStyling", "CreatedDateTime", "FlashcardTemplateName", "ModifiedDateTime", "NoteTypeID" },
                values: new object[] { 1, "{{Back}}", "{{Front}}", "", new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Default Flashcard", new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "NoteTypeFieldDefinition",
                columns: new[] { "NoteTypeFieldDefinitionID", "CreatedDateTime", "ModifiedDateTime", "NoteFieldDisplay", "NoteFieldName", "NoteTypeID", "SequenceID" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Front", "Front", 1, 1 });

            migrationBuilder.InsertData(
                table: "NoteTypeFieldDefinition",
                columns: new[] { "NoteTypeFieldDefinitionID", "CreatedDateTime", "ModifiedDateTime", "NoteFieldDisplay", "NoteFieldName", "NoteTypeID", "SequenceID" },
                values: new object[] { 2, new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 6, 30, 13, 9, 8, 289, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Back", "Back", 1, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Deck_DeckName",
                table: "Deck",
                column: "DeckName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deck_OboeteUserID",
                table: "Deck",
                column: "OboeteUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_FlashcardTemplateID",
                table: "Flashcard",
                column: "FlashcardTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_NoteID",
                table: "Flashcard",
                column: "NoteID");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardTemplate_NoteTypeID_FlashcardTemplateName",
                table: "FlashcardTemplate",
                columns: new[] { "NoteTypeID", "FlashcardTemplateName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_DeckID",
                table: "Note",
                column: "DeckID");

            migrationBuilder.CreateIndex(
                name: "IX_Note_NoteTypeID",
                table: "Note",
                column: "NoteTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NoteFieldValue_NoteId",
                table: "NoteFieldValue",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteFieldValue_NoteTypeFieldDefinitionId",
                table: "NoteFieldValue",
                column: "NoteTypeFieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteType_NoteTypeName",
                table: "NoteType",
                column: "NoteTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteType_OboeteUserID",
                table: "NoteType",
                column: "OboeteUserID");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTypeFieldDefinition_NoteTypeID_NoteFieldName",
                table: "NoteTypeFieldDefinition",
                columns: new[] { "NoteTypeID", "NoteFieldName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteTypeFieldDefinition_NoteTypeID_SequenceID",
                table: "NoteTypeFieldDefinition",
                columns: new[] { "NoteTypeID", "SequenceID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OboeteUserLoginToken_OboeteUserID",
                table: "OboeteUserLoginToken",
                column: "OboeteUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "NoteFieldValue");

            migrationBuilder.DropTable(
                name: "OboeteUserLoginToken");

            migrationBuilder.DropTable(
                name: "FlashcardTemplate");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "NoteTypeFieldDefinition");

            migrationBuilder.DropTable(
                name: "Deck");

            migrationBuilder.DropTable(
                name: "NoteType");

            migrationBuilder.DropTable(
                name: "OboeteUser");
        }
    }
}
