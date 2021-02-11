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
    public class LibraryConfigurationBusinessTests
    {
        private const string CUSTOMERID = "CUSTOMERID";
        private const string EMAIL = "a@gmail.com";
        
        private Mock<ILibraryManagementRepository> libraryManagementRepositoryMock;
        private LibraryConfigurationBusiness libraryConfigurationBusiness;

        [SetUp]
        public void SetUp()
        {
            libraryManagementRepositoryMock = new Mock<ILibraryManagementRepository>();
            libraryConfigurationBusiness = new LibraryConfigurationBusiness(libraryManagementRepositoryMock.Object);
        }

        [Test]
        public void CreateLibraryRegistry()
        {
            var allBooks = CreateAllBooks();
            var uniqueBooks = allBooks.GroupBy(a => a.Id).Count();
            var allUsers = CreateUsers();
            libraryManagementRepositoryMock.Setup(a => a.GetAllBooks()).Returns(allBooks);
            libraryManagementRepositoryMock.Setup(a => a.GetAllUsers()).Returns(allUsers);
            var libraryRegistry =  libraryConfigurationBusiness.CreateLibraryRegistry();


            Assert.AreEqual(libraryRegistry.BookRegistry.Count, uniqueBooks);
            Assert.AreEqual(libraryRegistry.UserRegistry.Count, allUsers.Count);
        }


        private List<User> CreateUsers()
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

        private List<Book> CreateAllBooks()
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
