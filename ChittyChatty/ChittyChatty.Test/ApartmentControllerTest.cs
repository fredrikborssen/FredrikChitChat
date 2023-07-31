using ChittyChatty.Controllers;
using ChittyChatty.Data;
using ChittyChatty.Domain.Entites;
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
        }
    }
}
