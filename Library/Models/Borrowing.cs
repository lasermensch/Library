using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Borrowing
    {
        [Required]
        public int InventoryID { get; set; } //FK
        [Required]
        public int BorrowerID { get; set; } //FK
        [Required]
        [Column(TypeName = "date")]
        public DateTime BorrowDate { get; set; }
        [Column(TypeName = "date")] //Behöver verkligen inte tiden för utlåning och återlämning mer exakt än datum.
        public DateTime? ReturnDate { get; set; } //Nullable: ReturnDate tilldelas först när föremålet lämnas tillbaka.
        public bool Rated { get; set; } = false; 
        [DefaultValue(null)]
        public virtual InventoryItem InventoryItem { get; set; } //NavProp
        [DefaultValue(null)]
        public virtual Borrower Borrower { get; set; } //NavProp
    }
}
