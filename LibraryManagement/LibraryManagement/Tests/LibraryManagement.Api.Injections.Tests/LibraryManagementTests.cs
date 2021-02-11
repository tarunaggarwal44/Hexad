using LibraryManagement.Api.Contracts.Interfaces;
using LibraryManagement.Api.Controllers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;


namespace Bookmyslot.Api.Injections.Tests
{
    public class LibraryManagementTests
    {
        private IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<LibraryManagement.Api.Startup>().Build();
            serviceProvider = webHost.Services;
            var configuration = serviceProvider.GetService<IConfiguration>();
            var Startup = new LibraryManagement.Api.Startup(configuration);
        }

        [Test]
        public void StartupTest()
        {
            var libraryManagementBusiness = serviceProvider.GetService<ILibraryManagementBusiness>();
            

            var controller = new LibraryManagementController(libraryManagementBusiness);

            Assert.IsNotNull(libraryManagementBusiness);
            Assert.IsNotNull(controller);
        }
    }
}