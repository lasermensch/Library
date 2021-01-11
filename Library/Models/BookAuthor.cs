using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookAuthor
    {
        //[Newtonsoft.Json.JsonIgnore] //Så att vi slipper se authorID och ISBN i onödan. Kopplingstabellen är ju inte nödvändig att visa,
                                     //så länge som c#-koden vet hur de andra kopplas. 
        public int AuthorID { get; set; } //FK
        //[Newtonsoft.Json.JsonIgnore] 
        [Column(TypeName ="char(13)")]
        public string ISBN { get; set; } //FK

        [DefaultValue(null)]
        public virtual Author Author { get; set; } //NavProp
        [DefaultValue(null)]
        public virtual Book Book { get; set; } //NavProp

        //Problemet är inte att den upprepar information, utan varje tur igenom ses som en unik sökväg...

        //public bool ShouldSerializeBook() //KANSKE denna lösning... Se https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        //{

        //    return (Book.BookAuthors.FirstOrDefault(a => a.ISBN != this.ISBN) != this);
        //}
    }
}
