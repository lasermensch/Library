using Bibliotek.Controllers;
using Bibliotek.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public bool Available { get; set; } = true; //hmm... I en eventuell framtida version är det värt att byta ut denna egenskap/kolumn...

        
        [DefaultValue(null)]
        public virtual Book Book { get; set; } //NavProp
        [DefaultValue(null)]
        public virtual Borrowing Borrowing { get; set; } //NavProp

    }
}
