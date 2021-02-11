using System.Collections.Generic;

namespace LibraryManagement.Api.Contracts.Interfaces
{
    public interface ILibraryRegistry
    {
        List<BookRegistry> BookRegistry { get; }

        List<UserRegistry> UserRegistry { get; }

        void SetUserRegistry(List<UserRegistry> userRegistries);

        void SetBookRegistry(List<BookRegistry> bookRegistries);
    }
}
