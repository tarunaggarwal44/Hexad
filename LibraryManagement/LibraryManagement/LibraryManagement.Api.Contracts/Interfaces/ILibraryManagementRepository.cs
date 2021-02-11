using System.Collections.Generic;

namespace LibraryManagement.Api.Contracts.Interfaces
{
    public interface ILibraryManagementRepository
    {
        List<Book> GetAllBooks();

        List<User> GetAllUsers();
    }
}
