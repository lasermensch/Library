using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookAuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthors()
        {
            return await _context.BookAuthors.ToListAsync();
        }

        // GET: api/BookAuthors/5
        [HttpGet("{authorid}/{isbn}")]
        public async Task<ActionResult<BookAuthor>> GetBookAuthor(int authorid, string isbn)
        {
            var bookAuthor = await _context.BookAuthors.FindAsync(authorid, isbn);
            
            if (bookAuthor == null)
            {
                return NotFound();
            }
            bookAuthor.Author = await _context.Authors.FirstOrDefaultAsync(a=>a.AuthorID == bookAuthor.AuthorID);
            bookAuthor.Book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == bookAuthor.ISBN);

            return bookAuthor;
        }

        // PUT: api/BookAuthors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{authorid}/{isbn}")]
        public async Task<IActionResult> PutBookAuthor(int authorid, string isbn, BookAuthor bookAuthor)
        {
            if (authorid != bookAuthor.AuthorID || isbn != bookAuthor.ISBN)
            {
                return BadRequest();
            }

            _context.Entry(bookAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorExists(authorid, isbn))
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

        // POST: api/BookAuthors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor bookAuthor)
        {
            if(bookAuthor.AuthorID == 0 && bookAuthor.Author != null)
            {
                Author author = bookAuthor.Author;
                bookAuthor.AuthorID = author.AuthorID;

                if (!_context.Authors.Any(a => a.AuthorID == author.AuthorID))
                {
                    await _context.Authors.AddAsync(author);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            if(bookAuthor.ISBN == null && bookAuthor.Book != null)
            {
                Book book = bookAuthor.Book;
                bookAuthor.ISBN = book.ISBN;

                if (!_context.Books.Any(b => b.ISBN == book.ISBN))
                {
                    await _context.Books.AddAsync(book);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            await _context.BookAuthors.AddAsync(bookAuthor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookAuthorExists(bookAuthor.AuthorID, bookAuthor.ISBN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // DELETE: api/BookAuthors/5
        [HttpDelete("{authorid}/{isbn}")]
        public async Task<ActionResult<BookAuthor>> DeleteBookAuthor(int authorid, string isbn)
        {
            var bookAuthor = await _context.BookAuthors.FindAsync(authorid, isbn);
            if (bookAuthor == null)
            {
                return NotFound();
            }
            
            _context.BookAuthors.Remove(bookAuthor);
            await _context.SaveChangesAsync();

            return bookAuthor;
        }

        private bool BookAuthorExists(int authorid, string isbn)
        {
            bool exists = false;
            if(_context.BookAuthors.Any(e=>e.AuthorID == authorid))
            {
                if(_context.BookAuthors.FirstOrDefault(ba=>ba.AuthorID == authorid).ISBN == isbn)
                {
                    exists = true;
                }
            }
            return exists;
        }

        
    }
}
