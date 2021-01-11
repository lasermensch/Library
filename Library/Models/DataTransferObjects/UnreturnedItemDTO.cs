using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.DataTransferObjects
{
    public class UnreturnedItemDTO //Best Practice är att inte skicka obehandlad information mellan api och mvc.
    {  
        //Borrower
        public string BorrowerID { get; set; }
        public string BorrowerFullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        //Book
        public string InventoryID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }

        //Borrowing
        public string BorrowDate { get; set; }
        public string ExpectedReturnDate { get; set; }

    }
}
