using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Borrowing
    {
        public int InventoryID { get; set; } //FK
        public int BorrowerID { get; set; } //FK
        [Required]
        [Column(TypeName = "date")]
        public DateTime BorrowDate { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; } //Det verkar som att variablers natur avgör om korresponderande kolumn i db blir nullable etc. Om inget annat är specificerat.
        public InventoryItem InventoryItem { get; set; } //NavProp
        public Borrower Borrower { get; set; } //NavProp
    }
}
