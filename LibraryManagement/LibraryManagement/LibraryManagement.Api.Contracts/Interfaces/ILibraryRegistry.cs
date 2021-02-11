using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Api.Contracts.Interfaces
{
    public interface ILibraryRegistry
    {
        List<BookRegistry> BookRegistry { get; }

        List<UserRegistry> UserRegistry { get; }
    }
}
