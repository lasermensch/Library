using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Library.Models
{
    public class InventoryItem
    {
        [Required]
        public int InventoryID { get; set; }
        [Column(TypeName = "char(13)")]
        [Required]
        public string ISBN { get; set; } //FK
        public bool Available { get; set; } = true; //När ett föremål läggs till är det bra om default-värdet är true på denna.

        
        [DefaultValue(null)]
        public virtual Book Book { get; set; } //NavProp
        [DefaultValue(null)]
        public virtual Borrowing Borrowing { get; set; } //NavProp

    }
}
