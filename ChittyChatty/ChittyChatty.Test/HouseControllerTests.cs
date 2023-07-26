using ChittyChatty.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ChittyChatty.Domain.Entites;

namespace ChittyChatty.Test
{
    public class HouseControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HouseControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        protected ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetHouses_ShouldReturnHouses_WhenHousesExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            var hus = new House(Guid.NewGuid(),"strand", 2, 34, DateTime.Now, "StensAB");
            dbContext.Houses.Add(hus);
            dbContext.SaveChanges();
           

            using var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ApplicationDbContext>(dbContext);
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("api/house");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
        }

    }
}