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

        // GET: api/Borrowings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Borrowing>> GetBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);

            if (borrowing == null)
            {
                return NotFound();
            }

            return borrowing;
        }

        // PUT: api/Borrowings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrowing(int id, Borrowing borrowing)
        {
            if (id != borrowing.BorrowerID)
            {
                return BadRequest();
            }

            _context.Entry(borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(id))
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
            _context.Borrowings.Add(borrowing);

            try
            {
                await _context.SaveChangesAsync();
                InventoryItem i = _context.InventoryItems.FirstOrDefault(i => i.InventoryID == borrowing.InventoryID); //För att se till att 
                i.Available = false;                                                          //den aktuella boken står som icke tillgänglig
                _context.InventoryItems.Update(i);                            //Note to self: se till att den flaggas om vid returndate...
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BorrowingExists(borrowing.BorrowerID))
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

        // DELETE: api/Borrowings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Borrowing>> DeleteBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing == null)
            {
                return NotFound();
            }

            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();

            return borrowing;
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowings.Any(e => e.BorrowerID == id);
        }
    }
}
