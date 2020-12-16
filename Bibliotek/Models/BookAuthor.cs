using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class BookAuthor
    {
        public int AuthorID { get; set; } //FK
        [Column(TypeName ="char(13)")]
        public string ISBN { get; set; } //FK
        public Author Author { get; set; } //NavProp
        public Book Book { get; set; } //NavProp

    }
}
