using Bibliotek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryDbContext _context)
        {
            _context.Database.EnsureCreated();

            if(!_context.Authors.Any())
            {
                Author[] authors = new Author[]
                {
                    new Author {FirstName="Stephen", LastName = "King"},
                    new Author {FirstName="JK", LastName="Rowling"},
                    new Author {FirstName="Håkan", LastName = "Nesser"}
                };
                foreach (Author a in authors)
                {
                    _context.Authors.Add(a);
                }
                _context.SaveChanges();
            }
            
            if (!_context.Books.Any())
            {
                Book[] Books = new Book[]
                {
                    new Book {ISBN = "9781501156700", Title ="Pet Sematary", YearOfPublication=1983},
                    new Book {ISBN = "9780385086950", Title="Carrie", YearOfPublication=1974},
                    new Book {ISBN = "9780747532743", Title = "Philosopher's Stone", YearOfPublication=1997},
                    new Book {ISBN = "9789173133173", Title = "Från doktor Klimkes horisont", YearOfPublication=2005}
                };
                foreach (Book b in Books)
                {
                    _context.Books.Add(b);
                }
                _context.SaveChanges();

            }

            if (!_context.Borrowers.Any())
            {
                Borrower[] borrowers = new Borrower[]
                {
                    new Borrower{FirstName="Fredrik", LastName="Lindroth"},
                    new Borrower{FirstName="Jonatan", LastName="Hallenberg"},
                    new Borrower{FirstName="Erik", LastName="Sundberg"}
                };
                foreach (Borrower b in borrowers)
                {
                    _context.Borrowers.Add(b);
                }
                _context.SaveChanges();
            }

            if (!_context.BookAuthors.Any())
            {
                BookAuthor[] bookAuthors = new BookAuthor[]
                {
                    new BookAuthor{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Pet Sematary").ISBN, AuthorID = _context.Authors.FirstOrDefault(a=>a.LastName == "King" && a.FirstName== "Stephen").AuthorID},
                    new BookAuthor{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Carrie").ISBN, AuthorID = _context.Authors.FirstOrDefault(a=>a.LastName == "King" && a.FirstName== "Stephen").AuthorID},
                    new BookAuthor{ ISBN = _context.Books.FirstOrDefault(b => b.Title == "Philosopher's Stone").ISBN, AuthorID = _context.Authors.FirstOrDefault(a => a.LastName == "Rowling" && a.FirstName == "JK").AuthorID },
                    new BookAuthor{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Från doktor Klimkes horisont").ISBN, AuthorID = _context.Authors.FirstOrDefault(a=>a.LastName == "Nesser" && a.FirstName== "Håkan").AuthorID},
                };
                foreach (BookAuthor ba in bookAuthors)
                {
                    _context.BookAuthors.Add(ba);
                }
                _context.SaveChanges();
            }

            if (!_context.InventoryItems.Any())
            {
                InventoryItem[] inventoryItems = new InventoryItem[]
                {
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Pet Sematary").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Pet Sematary").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Pet Sematary").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Carrie").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Carrie").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Philosopher's Stone").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Philosopher's Stone").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Från doktor Klimkes horisont").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Från doktor Klimkes horisont").ISBN, Available=true},
                    new InventoryItem{ISBN=_context.Books.FirstOrDefault(b=>b.Title=="Från doktor Klimkes horisont").ISBN, Available=true},
                };
                foreach (InventoryItem i in inventoryItems)
                {
                    _context.InventoryItems.Add(i);
                }
                _context.SaveChanges();
            }

            if (!_context.Borrowings.Any())
            {
                Borrowing[] borrowings = new Borrowing[]
                {
                    new Borrowing{BorrowerID=_context.Borrowers.FirstOrDefault(b => b.FirstName == "Fredrik" && b.LastName == "Lindroth").BorrowerID, 
                                InventoryID = _context.InventoryItems.FirstOrDefault(i => i.ISBN==(_context.Books.FirstOrDefault(b=>b.Title=="Philosopher's Stone").ISBN)).InventoryID,
                                BorrowDate = DateTime.Now}
                };

                foreach (Borrowing b in borrowings) //Ifall jag vill lägga in flera i seedningen i framtiden... 
                {
                    _context.Borrowings.Add(b);
                    InventoryItem i = _context.InventoryItems.FirstOrDefault(i => i.InventoryID == b.InventoryID);
                    i.Available = false;
                    _context.InventoryItems.Update(i);
                }

                _context.SaveChanges();
            }
            
        }
    }
}
