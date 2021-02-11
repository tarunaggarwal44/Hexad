using LibraryManagement.Api.Common.Contracts;
using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Constants;
using LibraryManagement.Api.Contracts.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Api.Business
{
    [TestFixture]
    public class LibraryManagementBusinessTests
    {
        private const string CUSTOMERID = "CUSTOMERID";
        private const string EMAIL = "a@gmail.com";

        private ILibraryRegistry libraryRegistry;
        private LibraryManagementBusiness libraryManagementBusiness;

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task GetAllAvailableBooks_BooksAvailable_ReturnsSuccessResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);

            var availableBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();


            Assert.AreEqual(availableBooksResponse.ResultType, ResultType.Success);
            Assert.AreEqual(availableBooksResponse.Result.Count, uniqueBooksCount);
        }

        [Test]
        public async Task GetAllAvailableBooks_1BooksAvailable1BookBorrowed_ReturnsEmptyResponse()
        {
            var allUsers = GetAllUsers();
            var book = GetAllBooks().FirstOrDefault();
            this.libraryRegistry = new LibraryRegistry(allUsers, new List<Book>() { book });
            this.libraryRegistry.SetUserRegistry(GetUserRegistries(book.Id, book.Name));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);

            var availableBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();

            Assert.AreEqual(availableBooksResponse.ResultType, ResultType.Empty);
        }


        [Test]
        public async Task GetAllAvailableBooks_SomeBooksBorrowed_ReturnsRemainingAvailableBooksResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetUserRegistries(3, BookConstants.PathAhead));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var availableBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();

            Assert.AreEqual(availableBooksResponse.ResultType, ResultType.Success);
            Assert.AreEqual(availableBooksResponse.Result.Count, 2);
        }

        private List<UserRegistry> GetUserRegistries(int id, string bookName)
        {
            List<UserRegistry> userRegistries = new List<UserRegistry>();
            var userRegistry = new UserRegistry();
            userRegistry.User = new User() { Email = UserConstants.User1 };
            userRegistry.BorrowedBooks = new List<Book>() { new Book() { Id = id, Name = bookName } };
            userRegistries.Add(userRegistry);

            return userRegistries;
        }

        private List<User> GetAllUsers()
        {
            var users = new List<User>();

            var user = new User();
            user.Email = UserConstants.User1;
            users.Add(user);

            user = new User();
            user.Email = UserConstants.User2;
            users.Add(user);

            return users;
        }

        private List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            var book = new Book() { Id = 1, Name = BookConstants.Sapien };
            books.Add(book);
            book = new Book() { Id = 1, Name = BookConstants.Sapien };
            books.Add(book);
            book = new Book() { Id = 1, Name = BookConstants.Sapien };
            books.Add(book);

            book = new Book() { Id = 2, Name = BookConstants.ZeroToOne };
            books.Add(book);
            book = new Book() { Id = 2, Name = BookConstants.ZeroToOne };
            books.Add(book);
            book = new Book() { Id = 2, Name = BookConstants.ZeroToOne };
            books.Add(book);

            book = new Book() { Id = 3, Name = BookConstants.PathAhead };
            books.Add(book);
            return books;
        }

    }
}
