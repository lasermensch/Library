using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bibliotek.Data;
using Bibliotek.Models;

namespace Bibliotek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowersController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Borrowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers()
        {
            return await _context.Borrowers.Include(borrowers => borrowers.Borrowings)
                .ThenInclude(borrowings => borrowings.InventoryItem)
                .ThenInclude(inventoryItem => inventoryItem.Book)
                .ThenInclude(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author).ToListAsync();
        }

        // GET: api/Borrowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Borrower>> GetBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);

            if (borrower == null)
            {
                return NotFound();
            }
            borrower.Borrowings = await _context.Borrowings.Where(b => b.BorrowerID == borrower.BorrowerID)
                .Include(borrowing => borrowing.InventoryItem)
                .ThenInclude(inventoryItem => inventoryItem.Book)
                .ThenInclude(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author).ToListAsync();

            return borrower;
        }

        // PUT: api/Borrowers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrower(int id, Borrower borrower)
        {
            if (id != borrower.BorrowerID)
            {
                return BadRequest();
            }

            _context.Entry(borrower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowerExists(id))
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

        // POST: api/Borrowers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Borrower>> PostBorrower(Borrower borrower)
        {
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBorrower", new { id = borrower.BorrowerID }, borrower);
        }

        // DELETE: api/Borrowers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Borrower>> DeleteBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);
            if (borrower == null)
            {
                return NotFound();
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return borrower;
        }
        //POST api/borrowers/5/Borrowbook/
        [HttpPost("{borrowerid}/borrowbook/{isbn}")]
        public async Task<ActionResult<Borrower>> BorrowBook(int borrowerid, string isbn)
        {
            var borrower = await _context.Borrowers.FindAsync(borrowerid);
            var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.ISBN == isbn && i.Available);

            if (borrower == null)
            {
                return NotFound();
            }

            if (item == null)
            {
                var book = await _context.Books.FindAsync(isbn);

                return BadRequest($"Title: {book.Title}, isbn: {book.ISBN} is not available");
            }
            if (_context.Borrowings.Any(b => b.BorrowerID == borrowerid && b.InventoryItem.ISBN == isbn))
            {
                return BadRequest("This person has already borrowed this book."); //Kanske borde ändra på i framtiden. Detta får fungera som
            }                                                                     //en patch just nu.

            Borrowing borrowing = new Borrowing();
            borrowing.BorrowDate = DateTime.Now;
            borrowing.BorrowerID = borrower.BorrowerID;
            borrowing.InventoryID = item.InventoryID;
            borrowing.ReturnDate = null;

            item.Available = false;
            try
            {
                await _context.Borrowings.AddAsync(borrowing);
                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
        [HttpPost("{borrowerid}/returnbook/")]
        public async Task<ActionResult<Borrower>> ReturnBook(int borrowerid, Book book)
        {
            if(book.ISBN == null)
            {
                return NotFound("input books isbn");
            }
            
            var borrowing = await _context.Borrowings.FirstOrDefaultAsync(b=>b.BorrowerID == borrowerid && b.InventoryItem.ISBN == book.ISBN);
            if(borrowing == null)
            { 
                return NotFound(); 
            }
            if (borrowing.ReturnDate != null)
            {
                return BadRequest("Item already returned");
            }
            borrowing.ReturnDate = DateTime.Now;
            var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.InventoryID == borrowing.InventoryID);
            item.Available = true;
            if (book.Grade != null)
            {
                await LeaveAGrade(book.ISBN, book.Grade);
                borrowing.Rated = true;
            }
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                _context.Entry(borrowing).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
            return Ok();
        }
        private async Task LeaveAGrade(string isbn, int? grade)
        {
            var book = await _context.Books.FindAsync(isbn);
            var borrowings = await _context.Borrowings.Include(b => b.InventoryItem)
                .Where(b => b.InventoryItem.ISBN == isbn && b.ReturnDate != null && b.Rated == true).ToListAsync();
            int circulation = borrowings.Count;
            if (circulation < 2)
                circulation = 2;
             
            book.Grade = (grade + (book.Grade * (circulation - 1)) )/ circulation;
            book.Grade = book.Grade ?? grade; //Om ekvationen ovan blir null beror det på att det tidigare inte lämnats betyg

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private bool BorrowerExists(int id)
        {
            return _context.Borrowers.Any(e => e.BorrowerID == id);
        }

        
    }
}
