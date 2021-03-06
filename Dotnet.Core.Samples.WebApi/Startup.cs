using System.Diagnostics.CodeAnalysis;
using Dotnet.Core.Samples.WebApi.Models;
using Dotnet.Core.Samples.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dotnet.Core.Samples.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dotnet.Core.Samples.WebApi", Version = "v1" });
            });

            services.AddDbContext<BookContext>(configure =>
                configure.UseSqlite(@"Data Source=Data/Books.db")
            );

            services.AddScoped<IBookService, BookService>();
        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
                application.UseSwagger();
                application.UseSwaggerUI(options =>
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dotnet.Core.Samples.WebApi v1")
                );
            }

            application.UseHttpsRedirection();
            application.UseRouting();
            application.UseAuthorization();
            application.UseEndpoints(configure =>
                {
                    configure.MapControllers();
                });
        }
    }
}
