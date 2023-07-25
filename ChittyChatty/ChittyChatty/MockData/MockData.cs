using ChittyChatty.Data;
using ChittyChatty.Domain.Entites;
using System.Text.Json;

namespace ChittyChatty.MockData
{
    public static class MockData
    {
        private static ApplicationDbContext _dbContext;

        public static void Initialize(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static async Task MockDatabaseDataHouses()
        {
            using (var fileStream = File.OpenRead("MockData/Data/HouseMockData.json"))
            {
                var data = await JsonSerializer.DeserializeAsync<List<House>>(fileStream);
                await _dbContext.Houses.AddRangeAsync(data);
                await _dbContext.SaveChangesAsync();
            }
        }

        public static async Task MockDatabaseDataApartments()
        {
            using (var fileStream = File.OpenRead("MockData/Data/ApartmentMockData.json"))
            {
                var data = await JsonSerializer.DeserializeAsync<List<Apartment>>(fileStream);
                await _dbContext.Apartments.AddRangeAsync(data);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
