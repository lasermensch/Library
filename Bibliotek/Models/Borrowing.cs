using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Borrowing
    {
        public int InventoryID { get; set; } //FK
        public int BorrowerID { get; set; } //FK
        [Column(TypeName = "date")]
        public DateTime BorrowDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime ReturnDate { get; set; }
        public InventoryItem InventoryItem { get; set; } //NavProp
        public Borrower Borrower { get; set; } //NavProp
    }
}
