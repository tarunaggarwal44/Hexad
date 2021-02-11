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
        }


        private static void LibraryManagementRepositoryInjections(IServiceCollection services)
        {
        }
    }
}
