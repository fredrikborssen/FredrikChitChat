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
                Id = apartment.BuildingId,
                BrokerId = apartment.BrokerId,
                Location = apartment.Location,
                Rooms = apartment.Rooms,
                Size = apartment.Size,
                Published = apartment.Published,
                Publisher = apartment.BrokerCompany
            }).ToListAsync();      

            if (!searchResult.Any())
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApartmentRm),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetApartmentById(Guid id)
        {
            var searchResult = await _dbContext.Apartments
                .Where(apartment => apartment.BuildingId == id)
                .FirstOrDefaultAsync();

            if(searchResult == null) 
                return NotFound();

            return Ok(searchResult);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ApartmentRm>),StatusCodes.Status200OK)]
        public async Task<IEnumerable<ApartmentRm>> SearchApartment([FromQuery] ApartmentDto searchParameter)
        {
            IQueryable<Apartment> apartments = _dbContext.Apartments;

            if (!string.IsNullOrWhiteSpace(searchParameter.Location))
                apartments = apartments.Where(a => a.Location.Contains(searchParameter.Location));

            if (searchParameter.Rooms != 0)
                apartments = apartments.Where(a => a.Rooms >= searchParameter.Rooms);

            if (searchParameter.Size != 0)
                apartments = apartments.Where(a => a.Size >= searchParameter.Size);

            if (searchParameter.Published != null)
                apartments = apartments.Where(a => a.Published >= searchParameter.Published);

            var apartmentList = await apartments
                                .Select(apartment => new ApartmentRm()
                                {
                                    Id = apartment.BuildingId,
                                    BrokerId = apartment.BrokerId,
                                    Location = apartment.Location,
                                    Rooms = apartment.Rooms,
                                    Size = apartment.Size,
                                    Published = apartment.Published,
                                    Publisher = apartment.BrokerCompany
                                }).ToArrayAsync();

            return apartmentList;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostNewApartment(ApartmentDto apartment)
        {
            var newApartment = new Apartment(apartment.BrokerId,apartment.Location,apartment.Rooms,apartment.Size,apartment.Published,apartment.Publisher);
            await _dbContext.Apartments.AddAsync(newApartment);
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetApartmentById), new { id = newApartment.BuildingId }, newApartment);
        }
    }
}
