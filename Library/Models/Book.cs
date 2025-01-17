﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{

    public class Book
    {
        [Required]
        [Column(TypeName ="char(13)")] //För att koden inte skulle dumma sig totalt behövde jag ta bort alla migrations och göra en ny init. Därför är init så omfattande...
        public string ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int YearOfPublication { get; set; }
        public int? Grade { get; set; } //I framtiden: float?
        [DefaultValue(null)]
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }//NavProp
        [DefaultValue(null)]
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } //NavProp
    }
}
