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
    public class BorrowingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Borrowings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrowing>>> GetBorrowings()
        {
            return await _context.Borrowings.ToListAsync();
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
            if (borrowerid != borrowing.BorrowerID || inventoryid != borrowing.InventoryID)
            {
                return BadRequest();
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
            if(!item.Available)
            {
                return BadRequest(); //?? Är detta det bästa i detta fall?
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
