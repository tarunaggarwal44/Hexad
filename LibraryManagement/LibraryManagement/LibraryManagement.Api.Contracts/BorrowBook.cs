using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Api.Contracts
{
    public class BorrowBook
    {
        public string Email { get; set; }

        public int BookId { get; set; }
    }
}
