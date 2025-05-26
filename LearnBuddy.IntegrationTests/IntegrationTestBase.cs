using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartRealEstateManagement.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;
        private readonly WebApplicationFactory<Program> _factory;

        protected IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContextOptions<ApplicationDbContext>
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Remove all DbContext-related services
                    var dbContextDescriptors = services.Where(
                        d => d.ServiceType == typeof(ApplicationDbContext) || d.ServiceType.Name.Contains("DbContext")).ToList();
                    foreach (var dbContextDescriptor in dbContextDescriptors)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    // Register in-memory database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryTestDb");
                    });
                });
            });

            Client = _factory.CreateClient();
        }

        protected void ResetDatabase()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
