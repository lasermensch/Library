﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotek.Models
{
    public class Borrower
    {
        public int BorrowerID { get; set; }
        [Column(TypeName ="varchar(50)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(50)")]

        public string LastName { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
