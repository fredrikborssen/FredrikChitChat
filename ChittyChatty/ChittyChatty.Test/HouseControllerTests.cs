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
using Bogus;

namespace ChittyChatty.Test
{
    public class HouseControllerTests 
    {
        protected ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
            var hus = new House(Guid.NewGuid(), "strand", 2, 34, DateTime.Now, "StensAB");
            var husInfo = JsonSerializer.Serialize(hus);
            dbContext.Houses.Add(hus);
            dbContext.SaveChanges();

            // Act
            var response = await houseController.GetHouses() as OkObjectResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseContent = response.Value as IEnumerable<HouseRm>;
            Assert.NotNull(responseContent);

            await dbContext.Database.EnsureDeletedAsync();
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
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetHouse_ShouldReturnOnehouse_WhenHouseIdSent()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);
            var hus = new House(Guid.NewGuid(), "strand", 2, 34, DateTime.Now, "StensAB");
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
            Assert.Equal(hus.BuildingId, houseRm.BuildingId);
            await dbContext.Database.EnsureDeletedAsync();
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

            // Send the POST request
            var result = await houseController.PostNewHouse(hus) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task PostHouse_ShouldReturnCreatedAtAction_WhenOkHouseDtoSentAndBrokerExist()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);
            var brokerController = new BrokerController(dbContext);

            var broker = new BrokerDto
            {
                FirstName = "Sten",
                Surname = "Testsson",
                BrokerCompany = "SaltSjö AB",
                LastUpdate = DateTime.Now
            };
            var brokerResult = await brokerController.PostNewBroker(broker) as CreatedAtActionResult;
            Assert.Equal(StatusCodes.Status201Created, brokerResult?.StatusCode);
            var brokerRm = brokerResult?.Value as BrokerRm;

            var hus = new HouseDto
            {
                BrokerId = brokerRm.BrokerId,
                Location = "Stensö",
                Rooms = 4,
                Size = 45,
                Published = DateTime.Now,
                BrokerCompany = brokerRm.BrokerCompany
            };

            var houseResult = await houseController.PostNewHouse(hus) as CreatedAtActionResult;
            var houseRm = houseResult?.Value as HouseRm;
            Assert.Equal(StatusCodes.Status201Created, houseResult?.StatusCode);
            Assert.Equal(brokerRm.BrokerId, houseRm.BrokerId);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetHouses_ShouldReturnListOfHouses_WhenSearching()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var houseController = new HouseController(dbContext);
            var faker = new Faker<House>()
                .RuleFor(h => h.BuildingId, f => f.Random.Guid())
                .RuleFor(h => h.BrokerId, f => f.Random.Guid())
                .RuleFor(h => h.Location, f => f.Address.StreetAddress())
                .RuleFor(h => h.Rooms, f => f.Random.Number(1, 5))
                .RuleFor(h => h.Size, f => f.Random.Number(20, 200))
                .RuleFor(h => h.Published, f => f.Date.Past())
                .RuleFor(h => h.BrokerCompany, (f, h) =>
                {
                    if (f.IndexFaker < 3)
                    {
                        return "Stenslöv";
                    }
                    else
                    {
                        return f.Company.CompanyName();
                    }
                });

            var numberOfHousesToGenerate = 10;
            var randomHouses = faker.Generate(numberOfHousesToGenerate);
            await dbContext.Houses.AddRangeAsync(randomHouses);
            await dbContext.SaveChangesAsync();

            var searchParameters = new HouseDto
            {
                BrokerCompany = "Stenslöv"
            };
            var response = await houseController.SearchHouse(searchParameters) as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, response?.StatusCode);
            var responseContent = response?.Value as IEnumerable<HouseRm>;
            Assert.True(responseContent.All(h => h.BrokerCompany == "Stenslöv"));
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}