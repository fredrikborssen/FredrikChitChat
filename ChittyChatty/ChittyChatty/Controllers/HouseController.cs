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
        public async Task<IActionResult> GetApartments()
        {
            var searchResult = await _dbContext.Houses.Select(apartment => new HouseRm
            {
                Id = apartment.Id,
                Location = apartment.Location,
                Rooms = apartment.Rooms,
                Size = apartment.Size,
                Published = apartment.Published,
                Publisher = apartment.Publisher
            }).ToListAsync();

            if (!searchResult.Any())
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HouseRm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApartmentById(Guid id)
        {
            var searchResult = await _dbContext.Houses
                .Where(apartment => apartment.Id == id)
                .FirstOrDefaultAsync();

            if (searchResult == null)
                return NotFound();

            return Ok(searchResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostNewApartment(HouseDto house)
        {
            var newHouse = new House(house.Location, house.Rooms, house.Size, house.Published, house.Publisher);
            await _dbContext.Houses.AddAsync(newHouse);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetApartmentById), new { id = newHouse.Id }, newHouse);
        }
    }
}
