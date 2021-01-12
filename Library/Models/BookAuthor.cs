using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Library.Models
{
    public class BookAuthor
    {
        //På grund av en post-metoden i BookAuthorController kan vi inte sätta [Required] på ISBN och AuthorID. Den aktuella metoden har en annan lösning.
        public int AuthorID { get; set; } //FK
        
        [Column(TypeName = "char(13)")]
        public string ISBN { get; set; } //FK

        [DefaultValue(null)]
        public virtual Author Author { get; set; } //NavProp
        [DefaultValue(null)]
        public virtual Book Book { get; set; } //NavProp

    }
}
