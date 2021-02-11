using LibraryManagement.Api.Contracts.Constants;
using LibraryManagement.Api.Contracts.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement.Api.Contracts
{


    public class LibraryRegistry : ILibraryRegistry
    {
        private List<BookRegistry> bookRegistry;
        private List<UserRegistry> userRegistry;

        public LibraryRegistry(List<User> users, List<Book> books)
        {
            if (users == null)
            {
                users = this.GetAllUsers();
            }
            if (books == null)
            {
                books = this.GetAllBooks();
            }
            
            this.userRegistry = this.CreateUserRegistry(users);
            this.bookRegistry = this.CreateBookRegistry(books);
        }
        public List<BookRegistry> BookRegistry => this.bookRegistry;

        public List<UserRegistry> UserRegistry => this.userRegistry;

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
        
            book = new Book() { Id = 2, Name = BookConstants.ZeroToOne };
            books.Add(book);
            book = new Book() { Id = 2, Name = BookConstants.ZeroToOne };
            books.Add(book);
           

            book = new Book() { Id = 3, Name = BookConstants.PathAhead };
            books.Add(book);

            return books;
        }


        private List<UserRegistry> CreateUserRegistry(List<User> allUsers)
        {
            var userRegistries = new List<UserRegistry>();

            foreach (var allUser in allUsers)
            {
                var userRegistry = new UserRegistry();
                userRegistry.User = allUser;
                userRegistry.BorrowedBooks = new List<Book>();
                userRegistries.Add(userRegistry);
            }

            return userRegistries;
        }

        private List<BookRegistry> CreateBookRegistry(List<Book> allBooks)
        {
            var bookRegistries = new List<BookRegistry>();
            var groupBooksById = allBooks.GroupBy(a => a.Id);
            foreach (var groupBookById in groupBooksById)
            {
                var bookRegistry = new BookRegistry();
                bookRegistry.Book = allBooks.FirstOrDefault(a => a.Id == groupBookById.Key);
                var count = allBooks.Count(a => a.Id == groupBookById.Key);
                bookRegistry.Count = count;
                bookRegistries.Add(bookRegistry);
            }

            return bookRegistries;
        }

        public void SetUserRegistry(List<UserRegistry> userRegistries)
        {
            this.userRegistry = userRegistries;
        }

        public void SetBookRegistry(List<BookRegistry> bookRegistries)
        {
            this.bookRegistry = bookRegistries;
        }
    }


}
