using Bogus;
using ChittyChatty.Controllers;
using ChittyChatty.Data;
using ChittyChatty.Domain.Entites;
using ChittyChatty.Dtos;
using ChittyChatty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChittyChatty.Test
{
    public class ApartmentControllerTest
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
        public async Task GetApartments_ShouldReturnApartments_WhenApartmentsExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);
            var apartment = new Apartment(Guid.NewGuid(), "strand", 2, 34, DateTime.Now, "StensAB");
            dbContext.Apartments.Add(apartment);
            dbContext.SaveChanges();

            // Act
            var response = await apartmentController.GetApartments() as OkObjectResult;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var responseContent = response.Value as IEnumerable<ApartmentRm>;
            Assert.NotNull(responseContent);
            Assert.Contains(responseContent, a => a.BuildingId == apartment.BuildingId);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetApartments_ShouldReturnNotFound_WhenApartmentsDoNotExist()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);

            // Act
            var response = await apartmentController.GetApartments() as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetApartment_ShouldReturnOneapartment_WhenApartmentIdSent()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);
            var apartment = new Apartment(Guid.NewGuid(), "strand", 2, 34, DateTime.Now, "StensAB");
            dbContext.Apartments.Add(apartment);
            dbContext.SaveChanges();

            var existingsApartments = await dbContext.Apartments.FirstOrDefaultAsync();
            var apartmentId = existingsApartments?.BuildingId;
            var result = await apartmentController.GetApartmentById(apartmentId.GetValueOrDefault()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var apartmentRm = result.Value as ApartmentRm;
            Assert.NotNull(apartmentRm);
            Assert.Equal(apartment.BuildingId, apartmentRm.BuildingId);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task PostApartment_ShouldReturnBadRequest_WhenOkApartmentDtoSentButBrokerDoesNotExist()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);
            var apartment = new ApartmentDto
            {
                BrokerId = Guid.NewGuid(),
                Location = "Stensö",
                Rooms = 4,
                Size = 45,
                Published = DateTime.Now,
                BrokerCompany = "Malm Sälj"
            };

            // Send the POST request
            var result = await apartmentController.PostNewApartment(apartment) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task PostApartment_ShouldReturnCreatedAtAction_WhenOkApartmentDtoSentAndBrokerExist()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);
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

            var apartment = new ApartmentDto
            {
                BrokerId = brokerRm.BrokerId,
                Location = "Stensö",
                Rooms = 4,
                Size = 45,
                Published = DateTime.Now,
                BrokerCompany = brokerRm.BrokerCompany
            };

            var apartmentResult = await apartmentController.PostNewApartment(apartment) as CreatedAtActionResult;
            var apartmentRm = apartmentResult?.Value as ApartmentRm;
            Assert.Equal(StatusCodes.Status201Created, apartmentResult?.StatusCode);
            Assert.Equal(brokerRm.BrokerId, apartmentRm.BrokerId);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetApartments_ShouldReturnListOfApartments_WhenSearching()
        {
            using var dbContext = GetDbContext();
            await ResetDatabase(dbContext);
            var apartmentController = new ApartmentController(dbContext);
            var faker = new Faker<Apartment>()
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

            var numberOfApartments = 10;
            var randomApartments = faker.Generate(numberOfApartments);
            await dbContext.Apartments.AddRangeAsync(randomApartments);
            await dbContext.SaveChangesAsync();

            var searchParameters = new ApartmentDto
            {
                BrokerCompany = "Stenslöv"
            };
            var response = await apartmentController.SearchApartment(searchParameters) as OkObjectResult;

            Assert.Equal(StatusCodes.Status200OK, response?.StatusCode);
            var responseContent = response?.Value as IEnumerable<ApartmentRm>;
            Assert.True(responseContent.All(h => h.BrokerCompany == "Stenslöv"));
            await dbContext.Database.EnsureDeletedAsync();
        }

    }
}
