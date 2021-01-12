using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Author
    {
        [Required]
        public int AuthorID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName ="varchar(50)")]
        public string LastName { get; set; }

        [DefaultValue(null)]
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } //Nav-prop

        //Metod för att lösa med kontrakt tagen och modifierad från https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        
    }
}
