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
    public class InventoryItemsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public InventoryItemsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/InventoryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
        {
            var inventoryItems = await _context.InventoryItems
                .Include(inventoryItems=>inventoryItems.Book)
                .ThenInclude(book=>book.BookAuthors).ToListAsync();
            foreach (InventoryItem i in inventoryItems)
            {
                foreach (BookAuthor ba in i.Book.BookAuthors)
                {
                    Author a = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorID == ba.AuthorID);
                    ba.Author.AuthorID = a.AuthorID;
                    ba.Author.FirstName = a.FirstName;
                    ba.Author.LastName = a.LastName;
                    ba.Author.BookAuthors = null;
                }
                i.Book.InventoryItems = null;
                if(!i.Available)
                {
                    i.Borrowing = await _context.Borrowings.FirstOrDefaultAsync(b=>b.InventoryID == i.InventoryID);
                    i.Borrowing.Borrower = await _context.Borrowers.FindAsync(i.Borrowing.BorrowerID);
                }
            }
            return inventoryItems;
        }

        // GET: api/InventoryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            inventoryItem.Book = await _context.Books.FindAsync(inventoryItem.ISBN);
            inventoryItem.Book.BookAuthors = await _context.BookAuthors.Where(bookAuthor => bookAuthor.ISBN == inventoryItem.ISBN)
                .Include(bookAuthor => bookAuthor.Author).ToListAsync();

            return inventoryItem;
        }

        // PUT: api/InventoryItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryItem(int id, InventoryItem inventoryItem)
        {
            if (id != inventoryItem.InventoryID)
            {
                return BadRequest();
            }

            _context.Entry(inventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
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

        // POST: api/InventoryItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Add(inventoryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryItem", new { id = inventoryItem.InventoryID }, inventoryItem);
        }

        // DELETE: api/InventoryItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryItem>> DeleteInventoryItem(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(inventoryItem);
            await _context.SaveChangesAsync();

            return inventoryItem;
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.InventoryID == id);
        }
    }
}
