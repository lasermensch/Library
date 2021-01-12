using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Library.Models
{
    public class Borrower
    {
        [Required]
        public int BorrowerID { get; set; }
        [Required]
        [Column(TypeName ="varchar(50)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        [DefaultValue(null)]
        public virtual ICollection<Borrowing> Borrowings { get; set; } //NavProp
    }
}
