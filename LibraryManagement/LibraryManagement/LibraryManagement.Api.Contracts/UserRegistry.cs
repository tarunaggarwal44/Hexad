using System.Collections.Generic;

namespace LibraryManagement.Api.Contracts
{
    public class UserRegistry
    {
        public User User { get; set; }

        public List<Book> BorrowedBooks { get; set; }
    }
}
