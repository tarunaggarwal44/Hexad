﻿using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Interfaces;
using LibraryManagement.Api.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get()
        {
            var getBooksResponse = await this.libraryManagementBusiness.GetAllAvailableBooks();
            return this.CreateGetHttpResponse(getBooksResponse);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post(BorrowBook borrowBook)
        {
            var getBooksResponse = await this.libraryManagementBusiness.BorrowBook(borrowBook.Email, borrowBook.BookId);
            return this.CreateGetHttpResponse(getBooksResponse);
        }
    }
}
