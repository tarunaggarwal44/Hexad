using LibraryManagement.Api.Common.Contracts;
using LibraryManagement.Api.Common.Contracts.Constants;
using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Constants;
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
            List<Book> allBorrowedBooks = GetAllBorrowedBooks();
            var bookRegisties = this.libraryRegistry.BookRegistry;
            List<Book> allBooks = GetAllBooksInLibrary(bookRegisties);

            var availableBooks = GetAvailableBooks(allBorrowedBooks, allBooks);

            if (availableBooks.Count == 0)
            {
                return new Response<List<Book>>() { ResultType = ResultType.Empty, Messages = new List<string>() { "No Books Available" } };
            }



            var response = new Response<List<Book>>() { Result = availableBooks };
            return await Task.FromResult(response);
        }

        private static List<Book> GetAllBooksInLibrary(List<BookRegistry> bookRegisties)
        {
            var allBooks = new List<Book>();
            foreach (var bookRegistry in bookRegisties)
            {
                for (int i = 0; i < bookRegistry.Count; i++)
                {
                    allBooks.Add(bookRegistry.Book);
                }
            }

            return allBooks;
        }

        private List<Book> GetAvailableBooks(List<Book> allBorrowedBooks, List<Book> allBooks)
        {
            var availableBooks = new List<Book>();
            var allBooksWithCopies = GenerateEachBookWithCopies(allBooks);
            var allBorrowedBooksWithCopies = GenerateEachBookWithCopies(allBorrowedBooks);

            foreach (var borrowedBookWithCopies in allBorrowedBooksWithCopies)
            {
                var borrowedCopies = allBorrowedBooksWithCopies[borrowedBookWithCopies.Key];
                var booksWithCopies = allBooksWithCopies[borrowedBookWithCopies.Key];
                allBooksWithCopies[borrowedBookWithCopies.Key] = booksWithCopies - borrowedCopies;
            }

            foreach (var bookWithCopies in allBooksWithCopies)
            {
                if (allBooksWithCopies[bookWithCopies.Key] > 0)
                {
                    availableBooks.Add(allBooks.FirstOrDefault(a => a.Id == bookWithCopies.Key));
                }
            }

            return availableBooks;
        }

        private List<Book> GetAllBorrowedBooks()
        {
            var getUsersWithBorrowedBooks = this.libraryRegistry.UserRegistry.Where(a => a.BorrowedBooks.Count > 0).ToList();

            var allBorrowedBooks = new List<Book>();
            foreach (var getUsersWithBorrowedBook in getUsersWithBorrowedBooks)
            {
                allBorrowedBooks.AddRange(getUsersWithBorrowedBook.BorrowedBooks);
            }

            return allBorrowedBooks;
        }

        private Dictionary<int, int>  GenerateEachBookWithCopies(List<Book> books)
        {
            var allBooksWithCopies = new Dictionary<int, int>();
            foreach (var book in books)
            {
                if (!allBooksWithCopies.ContainsKey(book.Id))
                {
                    allBooksWithCopies.Add(book.Id, 1);
                }

                else
                {
                    var copies = allBooksWithCopies[book.Id];
                    copies = copies + 1;
                    allBooksWithCopies[book.Id] = copies;
                }
            }

            return allBooksWithCopies;
        }

        public async Task<Response<bool>> BorrowBook(string email, int bookId)
        {
            var userRegistry = this.libraryRegistry.UserRegistry;
            var user = userRegistry.Where(a => a.User.Email == email).FirstOrDefault();

            var validateBookBorrowingResponse = await ValidateIfUserCanBorrow(user, bookId);
            if(validateBookBorrowingResponse.ResultType == ResultType.Success)
            {
                user.BorrowedBooks.Add(validateBookBorrowingResponse.Result);
                this.libraryRegistry.SetUserRegistry(userRegistry);

                return new Response<bool>() { Result = true };
            }

            return new Response<bool>() { ResultType = validateBookBorrowingResponse.ResultType, Messages = validateBookBorrowingResponse.Messages };
        }

        private async Task<Response<Book>> ValidateIfUserCanBorrow(UserRegistry userRegistry, int bookId)
        {
            if (userRegistry == null)
            {
                return new Response<Book>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessConstants.UserDoesntExists } };
            }

            var userAlreadyHasSameBook = userRegistry.BorrowedBooks.FirstOrDefault(a => a.Id == bookId);
            if (userAlreadyHasSameBook != null)
            {
                return new Response<Book>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessConstants.UserAlreadyHasSameBook } };
            }


            var alreadyBorrowedBooksByUser = userRegistry.BorrowedBooks.Count;
            if (alreadyBorrowedBooksByUser >= BookConstants.MaxBooksBorrowingAllowed)
            {
                return new Response<Book>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessConstants.BookBorrowingMaxLimitExceeded } };
            }

            var getAvailableBooks = await this.GetAllAvailableBooks();
            var bookToBorrow = getAvailableBooks.Result.Where(a => a.Id == bookId).FirstOrDefault();
            if (bookToBorrow == null)
            {
                return new Response<Book>() { ResultType = ResultType.ValidationError, Messages = new List<string>() { AppBusinessConstants.BookDoesntExists } };
            }

            return new Response<Book>() { Result = bookToBorrow };
        }
    }
}
