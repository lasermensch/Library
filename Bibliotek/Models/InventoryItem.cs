using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class InventoryItem
    {
        public int InventoryID { get; set; }
        [Column(TypeName = "char(13)")]
        public string ISBN { get; set; } //FK
        public bool Available { get; set; } //hmm...
        public Book Book { get; set; } //NavProp
        public Borrowing Borrowing { get; set; } //NavProp

    }
}
