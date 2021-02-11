using LibraryManagement.Api.Business;
using LibraryManagement.Api.Contracts;
using LibraryManagement.Api.Contracts.Interfaces;
using LibraryManagement.Api.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Injections
{
    public class LibraryManagementInjection
    {
        public static void LoadInjections(IServiceCollection services)
        {
            LibraryManagementBusinessInjections(services);
            LibraryManagementRepositoryInjections(services);
        }

        private static void LibraryManagementBusinessInjections(IServiceCollection services)
        {
            services.AddTransient<ILibraryManagementBusiness, LibraryManagementBusiness>();
            services.AddSingleton<ILibraryRegistry, LibraryRegistry>();

        }


        private static void LibraryManagementRepositoryInjections(IServiceCollection services)
        {
            services.AddTransient<ILibraryManagementRepository, LibraryManagementRepository>();
        }
    }
}
