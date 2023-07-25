using ChittyChatty.Data;
using ChittyChatty.Domain.Entites;
using ChittyChatty.Dtos;
using ChittyChatty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChittyChatty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HouseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<HouseRm>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHouses()
        {
            var searchResult = await _dbContext.Houses.Select(house => new HouseRm
            {
                Id = house.BuildingId,
                BrokerId = house.BrokerId,
                Location = house.Location,
                Rooms = house.Rooms,
                Size = house.Size,
                Published = house.Published,
                Publisher = house.BrokerCompany
            }).ToListAsync();

            if (!searchResult.Any())
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HouseRm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHouseById(Guid id)
        {
            var searchResult = await _dbContext.Houses
                .Where(house => house.BuildingId == id)
                .FirstOrDefaultAsync();

            if (searchResult == null)
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<HouseRm>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<HouseRm>> SearchHouse([FromQuery] HouseDto searchParameter)
        {
            IQueryable<House> houses = _dbContext.Houses;

            if (!string.IsNullOrWhiteSpace(searchParameter.Location))
                houses = houses.Where(a => a.Location.Contains(searchParameter.Location));

            if (searchParameter.Rooms != 0)
                houses = houses.Where(a => a.Rooms >= searchParameter.Rooms);

            if (searchParameter.Size != 0)
                houses = houses.Where(a => a.Size >= searchParameter.Size);

            if (searchParameter.Published != null)
                houses = houses.Where(a => a.Published >= searchParameter.Published);

            var houseList = await houses
                                .Select(house => new HouseRm()
                                {
                                    Id = house.BuildingId,
                                    BrokerId = house.BrokerId,
                                    Location = house.Location,
                                    Rooms = house.Rooms,
                                    Size = house.Size,
                                    Published = house.Published,
                                    Publisher = house.BrokerCompany
                                }).ToArrayAsync();

            return houseList;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostNewHouse(HouseDto house)
        {
            var newHouse = new House(house.BrokerId,house.Location, house.Rooms, house.Size, house.Published, house.Publisher);
            await _dbContext.Houses.AddAsync(newHouse);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetHouseById), new { id = newHouse.BuildingId }, newHouse);
        }
    }
}
