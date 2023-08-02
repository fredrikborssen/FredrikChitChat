using Bogus;
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
    public class BrokerControllerTest
    {
        protected ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetBroker_ShouldReturnBrokers_WhenBrokersExists()
        {
            using var dbContext = GetDbContext();
            var brokerController = new BrokerController(dbContext);
            var faker = new Faker<Broker>()
                .RuleFor(b => b.BrokerId, f => f.Random.Guid())
                .RuleFor(b => b.FirstName, f => f.Name.FirstName())
                .RuleFor(b => b.Surname, f => f.Name.LastName())
                .RuleFor(b => b.BrokerCompany, f => f.Company.CompanyName())
                                .RuleFor(h => h.BrokerCompany, f =>
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
            var numberBrokers = faker.Generate(10);
            await dbContext.Brokers.AddRangeAsync(numberBrokers);
            await dbContext.SaveChangesAsync();

            var result = await brokerController.GetBrokers() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
            var brokerLists = result?.Value as IEnumerable<BrokerRm>;
            Assert.True(brokerLists.Any(h => h.BrokerCompany == "Stenslöv"));
            Assert.Equal(10, brokerLists?.Count());
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetBrokerById_ShouldReturnSpecificBroker_WhenBrokerIdSent()
        {
            using var dbContext = GetDbContext();
            var brokerController = new BrokerController(dbContext);
            var faker = new Faker<Broker>()
                .RuleFor(b => b.BrokerId, f => f.Random.Guid())
                .RuleFor(b => b.FirstName, f => f.Name.FirstName())
                .RuleFor(b => b.Surname, f => f.Name.LastName())
                .RuleFor(b => b.BrokerCompany, f => f.Company.CompanyName())
                                .RuleFor(h => h.BrokerCompany, f =>
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
            var numberBrokers = faker.Generate(10);
            await dbContext.Brokers.AddRangeAsync(numberBrokers);
            await dbContext.SaveChangesAsync();

            var result = await brokerController.GetBrokerById(numberBrokers[0].BrokerId) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
            var broker = result?.Value as BrokerRm;
            Assert.Equal("Stenslöv", broker?.BrokerCompany);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task DeleteBrokerById_ShouldReturnNoContent_WhenBrokerIdSent()
        {
            using var dbContext = GetDbContext();
            var brokerController = new BrokerController(dbContext);
            var faker = new Faker<Broker>()
                .RuleFor(b => b.BrokerId, f => f.Random.Guid())
                .RuleFor(b => b.FirstName, f => f.Name.FirstName())
                .RuleFor(b => b.Surname, f => f.Name.LastName())
                .RuleFor(b => b.BrokerCompany, f => f.Company.CompanyName())
                                .RuleFor(h => h.BrokerCompany, f =>
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
            var numberBrokers = faker.Generate(10);
            await dbContext.Brokers.AddRangeAsync(numberBrokers);
            await dbContext.SaveChangesAsync();

            var result = await brokerController.DeleteBrokerById(numberBrokers[0].BrokerId) as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, result?.StatusCode);

            var brokers = await brokerController.GetBrokers() as OkObjectResult;
            Assert.NotNull(brokers);
            Assert.Equal(StatusCodes.Status200OK, brokers?.StatusCode);
            var brokerLists = brokers?.Value as IEnumerable<BrokerRm>;
            Assert.Equal(9, brokerLists?.Count());
            await dbContext.Database.EnsureDeletedAsync();
        }

    }
}
