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
    public class ApartmentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ApartmentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ApartmentRm>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApartments()
        {
            var searchResult = await _dbContext.Apartments.Select(apartment => new ApartmentRm
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostNewApartment(ApartmentDto apartment)
        {
            var newApartment = new Apartment(apartment.Location,apartment.Rooms,apartment.Size,apartment.Published,apartment.Publisher);
            await _dbContext.Apartments.AddAsync(newApartment);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
