using LibraryManagement.Api.Common.Contracts;
using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Constants;
using LibraryManagement.Api.Contracts.Interfaces;
using Moq;
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
        public void GetAllAvailableBooks_()
        {
            var users = GetAllUsers();
            var books  = GetAllBooks();
            this.libraryRegistry = new LibraryRegistry(users, books);
            this.libraryManagementBusiness = new LibraryManagementBusiness(this.libraryRegistry);

            var response = this.libraryManagementBusiness.GetAllAvailableBooks();


            //Assert.AreEqual(libraryRegistry.BookRegistry.Count, uniqueBooks);
            //Assert.AreEqual(libraryRegistry.UserRegistry.Count, allUsers.Count);
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


            return books;
        }

    }
}
