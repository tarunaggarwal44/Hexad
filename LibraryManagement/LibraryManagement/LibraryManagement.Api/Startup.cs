using LibraryManagement.Api.Common.Web.ExceptionHandlers;
using LibraryManagement.Api.Contracts.Interfaces;
using LibraryManagement.Api.Injections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LibraryManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            Injections(services);

            services.AddControllers();

            SwaggerDocumentation(services);

            BadRequestConfiguration(services);
        }


        private static void Injections(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            LibraryManagementInjection.LoadInjections(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseHttpsRedirection();



            app.ConfigureGlobalExceptionHandler();

            app.UseRouting();


            app.UseOpenApi();
            app.UseSwaggerUi3();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

      

        private static void BadRequestConfiguration(IServiceCollection services)
        {
            services.AddMvc()
       .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
       .ConfigureApiBehaviorOptions(options =>
       {
           options.InvalidModelStateResponseFactory = context =>
           {
               var problems = new BadRequestExceptionHandler(context);
               return new BadRequestObjectResult(problems.ErrorMessages);
           };
       });
        }

        private static void SwaggerDocumentation(IServiceCollection services)
        {
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Library Management  API";
                    document.Info.Description = "Library Management API to manage library";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "TA",
                        Email = string.Empty,
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "",
                    };
                };
            });
        }

    }
}
