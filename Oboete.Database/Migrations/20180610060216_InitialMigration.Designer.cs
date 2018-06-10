﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oboete.Database;

namespace Oboete.Database.Migrations
{
    [DbContext(typeof(OboeteContext))]
    [Migration("20180610060216_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Oboete.Database.Entity.FlashcardEntity", b =>
                {
                    b.Property<int>("FlashCardId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateDateTime")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("EaseFactor");

                    b.Property<int>("IntervalIndex");

                    b.Property<DateTimeOffset>("LastReviewDateTime");

                    b.Property<DateTimeOffset>("ModifiedDateTime");

                    b.Property<DateTimeOffset>("NextReviewDateTime");

                    b.Property<int>("NoteId");

                    b.HasKey("FlashCardId");

                    b.HasIndex("NoteId");

                    b.ToTable("Flashcard");
                });

            modelBuilder.Entity("Oboete.Database.Entity.NoteEntity", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateDateTime")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("ModifiedDateTime");

                    b.HasKey("NoteId");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Oboete.Database.Entity.FlashcardEntity", b =>
                {
                    b.HasOne("Oboete.Database.Entity.NoteEntity", "Note")
                        .WithMany("FlashCard")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
