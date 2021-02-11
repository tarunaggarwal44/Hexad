using LibraryManagement.Api.Common.Contracts;
using LibraryManagement.Api.Common.Contracts.Constants;
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
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(book.Id, book.Name));
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
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(3, BookConstants.PathAhead));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var availableBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();

            Assert.AreEqual(availableBooksResponse.ResultType, ResultType.Success);
            Assert.AreEqual(availableBooksResponse.Result.Count, 2);
        }


        [Test]
        public async Task BorrowBook_UserDoesntExists_ReturnsUserDoesntExistsValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(3, BookConstants.PathAhead));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.BorrowBook("asas", 1);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.UserDoesntExists);
        }

        [Test]
        public async Task BorrowBook_UserBookBorrowingMaxLimitExceeded_ReturnsUserBookBorrowingMaxLimitExceededValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetWith2BookedBorrowedUserRegistries());
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.BorrowBook(UserConstants.User1, 3);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.BookBorrowingMaxLimitExceeded);
        }

        [Test]
        public async Task BorrowBook_UserAlreadyHasSameBook_ReturnsUserAlreadyHasSameBookValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(1, BookConstants.Sapien));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.BorrowBook(UserConstants.User1, 1);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.UserAlreadyHasSameBook);
        }


        [Test]
        public async Task BorrowBook_BookDoesntExists_ReturnsBookDoesntExistsValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.BorrowBook(UserConstants.User1 , 10);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.BookDoesntExists);
        }


        [Test]
        public async Task BorrowBook_UserCanBorrowBook_ReturnsSuccessResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.BorrowBook(UserConstants.User1, 1);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.Success);
            Assert.AreEqual(borrowBookResponse.Result, true);
        }



        [Test]
        public async Task ReturnBook_UserDoesntExists_ReturnsUserDoesntExistsValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.ReturnBook("asas", 1);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.UserDoesntExists);
        }

        [Test]
        public async Task ReturnBook_UserDoesntHaveBookToReturn_ReturnsUserDoesntHaveBookToReturnValidationResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(3, BookConstants.PathAhead));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.ReturnBook(UserConstants.User1, 10);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.ValidationError);
            Assert.AreEqual(borrowBookResponse.Messages.First(), AppBusinessConstants.UserDoesntHaveBookToReturn);
        }


        [Test]
        public async Task ReturnBook_UserHasBookToReturn_ReturnsBookSuccessfullyResponse()
        {
            var allUsers = GetAllUsers();
            var allBooks = GetAllBooks();
            var uniqueBooksCount = allBooks.GroupBy(a => a.Id).Count();
            this.libraryRegistry = new LibraryRegistry(allUsers, allBooks);
            this.libraryRegistry.SetUserRegistry(GetDefaultUserRegistries(3, BookConstants.PathAhead));
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);


            var borrowBookResponse = await this.libraryManagementBusiness.ReturnBook(UserConstants.User1, 3);

            Assert.AreEqual(borrowBookResponse.ResultType, ResultType.Success);
        }


        private List<UserRegistry> GetDefaultUserRegistries(int id, string bookName)
        {
            List<UserRegistry> userRegistries = new List<UserRegistry>();
            var userRegistry = new UserRegistry();
            userRegistry.User = new User() { Email = UserConstants.User1 };
            userRegistry.BorrowedBooks = new List<Book>() { new Book() { Id = id, Name = bookName } };
            userRegistries.Add(userRegistry);

            return userRegistries;
        }

        private List<UserRegistry> GetWith2BookedBorrowedUserRegistries()
        {
            List<UserRegistry> userRegistries = new List<UserRegistry>();
            var userRegistry = new UserRegistry();
            userRegistry.User = new User() { Email = UserConstants.User1 };
            userRegistry.BorrowedBooks = new List<Book>() {
                new Book() { Id = 1, Name = BookConstants.Sapien } ,new Book() { Id = 2, Name = BookConstants.ZeroToOne }
            };
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
