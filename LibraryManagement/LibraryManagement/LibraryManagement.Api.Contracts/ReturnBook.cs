using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Api.Contracts
{
    public class ReturnBook
    {
        public string Email { get; set; }

        public int BookId { get; set; }
    }
}
