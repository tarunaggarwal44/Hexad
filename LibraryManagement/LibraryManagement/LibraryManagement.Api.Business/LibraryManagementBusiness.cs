using LibraryManagement.Api.Common.Contracts;
using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Api.Business
{
    public class LibraryManagementBusiness : ILibraryManagementBusiness
    {
        private readonly ILibraryRegistry libraryRegistry;
        public LibraryManagementBusiness(ILibraryRegistry libraryRegistry)
        {
            this.libraryRegistry = libraryRegistry;
        }

       

        


        public async Task<Response<List<Book>>> GetAllAvailableBooks()
        {
            var getUsersWithBorrowedBooks = this.libraryRegistry.UserRegistry.Where(a => a.BorrowedBooks.Count > 0);

            var allBorrowedBooks = new List<Book>();
            foreach (var getUsersWithBorrowedBook in getUsersWithBorrowedBooks)
            {
                allBorrowedBooks.AddRange(getUsersWithBorrowedBook.BorrowedBooks);
            }

            var availableBooks = new List<Book>();
            var response =  new Response<List<Book>>() { Result = availableBooks };
            return await Task.FromResult(response);
        }


    }
}
