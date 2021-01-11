using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using SQLitePCL;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _context.Books.Include(book => book.BookAuthors)
                .ThenInclude(bookAuthor=>bookAuthor.Author).ToListAsync();
            
            var inventoryItems = await _context.InventoryItems.ToListAsync(); //Eftersom den behöver gå i två riktningar blir det inte bättre än om
                                                                              //man gör så här.
            foreach (Book b in books)
            {
                foreach (BookAuthor ba in b.BookAuthors)
                {
                    ba.Author.BookAuthors = null; //Enklaste sättet att hindra den från att spotta ut alla andra böcker som författaren har skrivit.
                }
                b.InventoryItems = inventoryItems.FindAll(i => i.ISBN == b.ISBN);
            }
         
            return books;
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null)
            {
                return NotFound();
            }

            book.BookAuthors = await _context.BookAuthors.Where(ba => ba.ISBN == id).ToListAsync();
            book.InventoryItems = await _context.InventoryItems.Where(i => i.ISBN == id).ToListAsync();
            foreach (BookAuthor ba in book.BookAuthors)
            {
                ba.Author = await _context.Authors.FindAsync(ba.AuthorID);
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(string id, Book book)
        {

            if (id != book.ISBN)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookExists(book.ISBN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBook", new { id = book.ISBN }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }
        

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.ISBN == id);
        }
    }
}
