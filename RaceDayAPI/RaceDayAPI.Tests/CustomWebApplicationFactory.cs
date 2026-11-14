using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using System.Linq;

namespace RaceDayAPI.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RaceDayDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<RaceDayDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_" + System.Guid.NewGuid().ToString());
                });
            });
        }
    }
}