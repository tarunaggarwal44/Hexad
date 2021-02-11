using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Interfaces;
using LibraryManagement.Api.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.Api.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class LibraryManagementController : BaseApiController
    {
        private readonly ILibraryManagementBusiness libraryManagementBusiness;
        public LibraryManagementController(ILibraryManagementBusiness libraryManagementBusiness)
        {
            this.libraryManagementBusiness = libraryManagementBusiness;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet()]
        [Route("api/v1/LibraryManagement")]
        public async Task<IActionResult> Get()
        {
            var getBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();
            return this.CreateGetHttpResponse(getBooksResponse);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("api/v1/LibraryManagement/BorrowBook")]

        public async Task<IActionResult> BorrowBook(BorrowBook borrowBook)
        {
            var getBorrowBookResponse = await this.libraryManagementBusiness.BorrowBook(borrowBook.Email, borrowBook.BookId);
            return this.CreateGetHttpResponse(getBorrowBookResponse);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("api/v1/LibraryManagement/ReturnBook")]

        public async Task<IActionResult> ReturnBook(ReturnBook returnBook)
        {
            var getReturnBookResponse = await this.libraryManagementBusiness.ReturnBook(returnBook.Email, returnBook.BookId);
            return this.CreateGetHttpResponse(getReturnBookResponse);
        }
    }
}
