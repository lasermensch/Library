using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        public int ISBN { get; set; }
        public bool Available { get; set; }
    }
}
