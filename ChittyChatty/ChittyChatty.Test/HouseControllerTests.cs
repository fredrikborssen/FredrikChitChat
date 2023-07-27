using ChittyChatty.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ChittyChatty.Domain.Entites;
using System.Text.Json;
using System.Net;
using ChittyChatty.Controllers;
using ChittyChatty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        protected HttpClient GetHttpClient(ApplicationDbContext dbContext)
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ApplicationDbContext>(dbContext);
                });
            }).CreateClient();

            return client;
        }



        [Fact]
        public async Task GetHouses_ShouldReturnHouses_WhenHousesExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            using var client = GetHttpClient(dbContext);
            var hus = new House(Guid.NewGuid(),"strand", 2, 34, DateTime.Now, "StensAB");
            var husInfo =  JsonSerializer.Serialize(hus);
            dbContext.Houses.Add(hus);
            dbContext.SaveChanges();


            // Act
            var response = await client.GetAsync("api/house");

            // Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent = responseContent.TrimStart('[').TrimEnd(']');
            Assert.Contains(responseContent, husInfo, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetHouses_ShouldReturnNotFound_WhenHousesDoNotExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            using var client = GetHttpClient(dbContext);

            // Act
            var response = await client.GetAsync("api/house");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }

        [Fact]
        public async Task GetHouse_ShouldReturnOnehouse_WhenHouseIdSent()
        {
            // Arrange
            using var dbContext = GetDbContext();
            var houseController = new HouseController(dbContext);
            var hus = new House(Guid.NewGuid(), "strand", 2, 34, DateTime.Now, "StensAB");
            var husInfo = JsonSerializer.Serialize(hus);
            dbContext.Houses.Add(hus);
            dbContext.SaveChanges();
            var existingHouse = await dbContext.Houses.FirstOrDefaultAsync();
            var houseId = existingHouse?.BuildingId;
            var result = await houseController.GetHouseById(houseId.GetValueOrDefault()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var houseRm = result.Value as HouseRm;
            Assert.NotNull(result.Value);
        }
    }
}