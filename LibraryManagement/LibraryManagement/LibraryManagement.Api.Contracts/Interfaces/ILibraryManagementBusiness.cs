using LibraryManagement.Api.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Api.Contracts.Interfaces
{
    public interface ILibraryManagementBusiness
    {
        Task<Response<List<Book>>> GetAllAvailableBooks();



    }
}
