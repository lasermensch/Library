using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Borrowing
    {
        public int InventoryID { get; set; } //FK
        public int BorrowerID { get; set; } //FK
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public InventoryItem InventoryItem { get; set; } //NavProp
        public Borrower Borrower { get; set; } //NavProp
    }
}
