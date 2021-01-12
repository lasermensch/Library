using Library.Data;
using Library.Models;
using Library.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingsController(LibraryDbContext context)
        {
            _context = context;
        }
        //GET: api/Borrowings/UnreturnedItems
        [HttpGet("unreturneditems")]
        public async Task<ActionResult<IEnumerable<UnreturnedItemDTO>>> GetUnreturnedItems()
        {
            var unreturnedItemDTOs = new List<UnreturnedItemDTO>();

            var borrowings = await _context.Borrowings.Where(b=>b.ReturnDate == null).Include(borrowing => borrowing.InventoryItem) //Skickar relevant information vid begäran.
                .ThenInclude(inventoryItem => inventoryItem.Book)
                .ThenInclude(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author).ToListAsync();
            foreach (Borrowing b in borrowings)
            {
                b.Borrower = await _context.Borrowers.FirstOrDefaultAsync(borrower => borrower.BorrowerID == b.BorrowerID);

                UnreturnedItemDTO dto = new UnreturnedItemDTO();
                dto.AuthorName = "";
                foreach (BookAuthor ba in b.InventoryItem.Book.BookAuthors)
                {
                    dto.AuthorName += $"{ba.Author.FirstName} {ba.Author.LastName}, ";
                }
                dto.AuthorName = dto.AuthorName.Trim(' ', ',');
                dto.ISBN = b.InventoryItem.ISBN;
                dto.Title = b.InventoryItem.Book.Title;
                dto.InventoryID = b.InventoryID.ToString();

                dto.BorrowDate = b.BorrowDate.ToString("yyyy-MM-dd");
                dto.ExpectedReturnDate = b.BorrowDate.AddDays(28).ToString("yyyy-MM-dd");

                dto.BorrowerID = b.BorrowerID.ToString();
                dto.BorrowerFullName = $"{b.Borrower.FirstName} {b.Borrower.LastName}";
                dto.Email = b.Borrower.Email;
                dto.PhoneNumber = b.Borrower.PhoneNumber;
                dto.Address = b.Borrower.Address;

                unreturnedItemDTOs.Add(dto);

            }

            return unreturnedItemDTOs;
        }

        // GET: api/Borrowings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrowing>>> GetBorrowings()
        {
            var borrowings = await _context.Borrowings.Include(borrowing=>borrowing.InventoryItem) //Skickar relevant information vid begäran.
                .ThenInclude(inventoryItem=>inventoryItem.Book)
                .ThenInclude(book=>book.BookAuthors)
                .ThenInclude(bookAuthor=>bookAuthor.Author).ToListAsync();
            foreach (Borrowing b in borrowings)
            {
                b.Borrower = await _context.Borrowers.FirstOrDefaultAsync(borrower => borrower.BorrowerID == b.BorrowerID);
            }
            return borrowings;
        }

        // GET: api/Borrowings/5/10
        [HttpGet("{borrowerid}/{inventoryid}")]
        public async Task<ActionResult<Borrowing>> GetBorrowing(int borrowerid, int inventoryid)
        {
            var borrowing = await _context.Borrowings.FindAsync(borrowerid, inventoryid);

            if (borrowing == null)
            {
                return NotFound();
            }

            return borrowing;
        }

        // PUT: api/Borrowings/5/10
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        
        [HttpPut("{borrowerid}/{inventoryid}")]
        public async Task<IActionResult> PutBorrowing(int borrowerid, int inventoryid, Borrowing borrowing)
        {
            if (borrowerid != borrowing.BorrowerID || inventoryid != borrowing.InventoryID || !ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(x => x.Value.Errors));
            }
            
            _context.Entry(borrowing).State = EntityState.Modified;
            InventoryItem item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.InventoryID == inventoryid);
            try
            {
                if (borrowing.ReturnDate != null) //I de allra flesta fall är nu boken återlämnad. 
                    item.Available = true;    //Skulle väl vara i ev. klientprogrammet som säkerheten gällande returdatum ska skötas?

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(inventoryid, borrowerid))
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

        // POST: api/Borrowings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Borrowing>> PostBorrowing(Borrowing borrowing)
        {
            InventoryItem item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.InventoryID == borrowing.InventoryID);
            if(borrowing == null || item == null)
            {
                return NotFound();
            }
            if(!item.Available)
            {
                return BadRequest("Item not available."); //?? Är detta det bästa i detta fall?
            }
            _context.Borrowings.Add(borrowing);

            try
            {
                 item.Available = false;                                                
                _context.InventoryItems.Update(item);                    
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BorrowingExists(borrowing.InventoryID, borrowing.BorrowerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBorrowing", new { id = borrowing.BorrowerID }, borrowing);
        }

        // DELETE: api/Borrowings/5/7
        [HttpDelete("{borrowerid}/{inventoryid}")]
        public async Task<ActionResult<Borrowing>> DeleteBorrowing(int borrowerid, int inventoryid)
        {
            var borrowing = await _context.Borrowings.FindAsync(borrowerid, inventoryid);
            if (borrowing == null)
            {
                return NotFound();
            }
            var item = await _context.InventoryItems.FindAsync(inventoryid);
            item.Available = true;
            _context.Entry(item).State = EntityState.Modified;

            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();

            return borrowing;
        }

        private bool BorrowingExists(int inventoryid, int borrowerid)
        {
            bool exists = false;
            if(_context.Borrowings.Any(e => e.BorrowerID == borrowerid))
            {
                if(_context.Borrowings.FirstOrDefault(e=>e.BorrowerID == borrowerid).InventoryID == inventoryid)
                {
                    exists = true;
                }
            }

            return exists;
        }
    }
}
