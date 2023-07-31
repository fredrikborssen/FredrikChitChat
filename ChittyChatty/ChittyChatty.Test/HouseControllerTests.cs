using ChittyChatty.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ChittyChatty.Domain.Entites;
using System.Text.Json;
using ChittyChatty.Controllers;
using ChittyChatty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChittyChatty.Dtos;
using System.Text;
using Azure;

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

        private async Task ResetDatabase(ApplicationDbContext dbContext)
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }


        [Fact]
        public async Task GetHouses_ShouldReturnHouses_WhenHousesExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);
            var hus = new House(Guid.NewGuid(),"strand", 2, 34, DateTime.Now, "StensAB");
            var husInfo =  JsonSerializer.Serialize(hus);
            dbContext.Houses.Add(hus);
            dbContext.SaveChanges();


            // Act
            var response = await houseController.GetHouses() as OkObjectResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseContent = response.Value as IEnumerable<HouseRm>;
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task GetHouses_ShouldReturnNotFound_WhenHousesDoNotExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);

            // Act
            var response = await houseController.GetHouses() as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, (response as NotFoundResult).StatusCode);
        }

        [Fact]
        public async Task GetHouse_ShouldReturnOnehouse_WhenHouseIdSent()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
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
            Assert.NotNull(houseRm);
        }

        [Fact]
        public async Task PostHouse_ShouldReturnBadRequest_WhenOkHouseDtoSentButBrokerDoesNotExist()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);
            var hus = new HouseDto
            {
                BrokerId = Guid.NewGuid(),
                Location = "Stensö",
                Rooms = 4,
                Size = 45,
                Published = DateTime.Now,
                BrokerCompany = "Malm Sälj"
            };
            var jsonContent = JsonSerializer.Serialize(hus);

            // Create a StringContent instance with JSON data
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the POST request
            var result = await houseController.PostNewHouse(hus) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}