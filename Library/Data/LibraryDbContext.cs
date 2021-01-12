using Library.Models;
using Microsoft.EntityFrameworkCore;



namespace Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {


        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.ISBN); //istället för att använda attribut i modellklassen.

            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.AuthorID, ba.ISBN });
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.ISBN);
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorID);

            modelBuilder.Entity<InventoryItem>().HasKey(i => i.InventoryID);
            modelBuilder.Entity<InventoryItem>().HasOne(i => i.Book)
                .WithMany(b => b.InventoryItems)
                .HasForeignKey(i => i.ISBN);

            modelBuilder.Entity<Borrowing>().HasKey(b => new { b.BorrowerID, b.InventoryID });
            modelBuilder.Entity<Borrowing>().HasOne(b => b.Borrower)
                .WithMany(b => b.Borrowings)
                .HasForeignKey(b => b.BorrowerID);
            modelBuilder.Entity<Borrowing>().HasOne(b => b.InventoryItem)
                .WithOne(i => i.Borrowing)
                .HasForeignKey<Borrowing>(b => b.InventoryID);//Blir fel i koden om man inte specificerar när det gäller one-to-one.
            modelBuilder.Entity<Borrowing>().HasIndex(b => b.InventoryID).IsUnique(false); //Den förutsatte att tabellen skulle indexeras på 
                                                                                          //inventoryID och vara unik. Denna inställning tar
                                                                                         //bort uniciteten. Jag känner inget behov av att bry mig
                                                                                          //om att den fortfarande indexerar på den egenskapen.

        }
        

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<BookAuthor>BookAuthors { get; set; }

    }
}
