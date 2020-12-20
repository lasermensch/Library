using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName ="varchar(50)")]
        public string LastName { get; set; }
        
        public virtual ICollection<BookAuthor> BookAuthors { get; set; } 

        //Metod för att lösa med kontrakt tagen och modifierad från https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        
    }
}
