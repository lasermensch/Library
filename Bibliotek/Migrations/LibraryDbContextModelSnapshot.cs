﻿// <auto-generated />
using System;
using Bibliotek.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bibliotek.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    partial class LibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bibliotek.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("AuthorID");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Bibliotek.Models.Book", b =>
                {
                    b.Property<string>("ISBN")
                        .HasColumnType("char(13)");

                    b.Property<int?>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("YearOfPublication")
                        .HasColumnType("int");

                    b.HasKey("ISBN");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Bibliotek.Models.BookAuthor", b =>
                {
                    b.Property<int>("AuthorID")
                        .HasColumnType("int");

                    b.Property<string>("ISBN")
                        .HasColumnType("char(13)");

                    b.HasKey("AuthorID", "ISBN");

                    b.HasIndex("ISBN");

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("Bibliotek.Models.Borrower", b =>
                {
                    b.Property<int>("BorrowerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("BorrowerID");

                    b.ToTable("Borrowers");
                });

            modelBuilder.Entity("Bibliotek.Models.Borrowing", b =>
                {
                    b.Property<int>("BorrowerID")
                        .HasColumnType("int");

                    b.Property<int>("InventoryID")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("date");

                    b.HasKey("BorrowerID", "InventoryID");

                    b.HasIndex("InventoryID")
                        .IsUnique();

                    b.ToTable("Borrowings");
                });

            modelBuilder.Entity("Bibliotek.Models.InventoryItem", b =>
                {
                    b.Property<int>("InventoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<string>("ISBN")
                        .HasColumnType("char(13)");

                    b.HasKey("InventoryID");

                    b.HasIndex("ISBN");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Bibliotek.Models.BookAuthor", b =>
                {
                    b.HasOne("Bibliotek.Models.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bibliotek.Models.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("ISBN")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bibliotek.Models.Borrowing", b =>
                {
                    b.HasOne("Bibliotek.Models.Borrower", "Borrower")
                        .WithMany("Borrowings")
                        .HasForeignKey("BorrowerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bibliotek.Models.InventoryItem", "InventoryItem")
                        .WithOne("Borrowing")
                        .HasForeignKey("Bibliotek.Models.Borrowing", "InventoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Bibliotek.Models.InventoryItem", b =>
                {
                    b.HasOne("Bibliotek.Models.Book", "Book")
                        .WithMany("InventoryItems")
                        .HasForeignKey("ISBN");
                });
#pragma warning restore 612, 618
        }
    }
}
