using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Book
    {
        
        [Column(TypeName ="char(13)")] //För att koden inte skulle dumma sig totalt behövde jag ta bort alla migrations och göra en ny init. Därför är init så omfattande...
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int YearOfPublication { get; set; }
        public int? Grade { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }//NavProp
        public ICollection<InventoryItem> InventoryItems { get; set; } //NavProp
    }
}
