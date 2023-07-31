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
                BuildingId = apartment.BuildingId,
                BrokerId = apartment.BrokerId,
                Location = apartment.Location,
                Rooms = apartment.Rooms,
                Size = apartment.Size,
                Published = apartment.Published,
                BrokerCompany = apartment.BrokerCompany
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

            if (!string.IsNullOrWhiteSpace(searchParameter.BrokerCompany))
                apartments = apartments.Where(a => a.BrokerCompany.Contains(searchParameter.BrokerCompany));

            var apartmentList = await apartments
                                .Select(apartment => new ApartmentRm()
                                {
                                    BuildingId = apartment.BuildingId,
                                    BrokerId = apartment.BrokerId,
                                    Location = apartment.Location,
                                    Rooms = apartment.Rooms,
                                    Size = apartment.Size,
                                    Published = apartment.Published,
                                    BrokerCompany = apartment.BrokerCompany
                                }).ToArrayAsync();

            return apartmentList;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostNewApartment(ApartmentDto apartment)
        {
            var broker = await _dbContext.Brokers.FindAsync(apartment.BrokerId);

            if (broker == null)
                return BadRequest("The Broker does not exist.");

            var newApartment = new Apartment(apartment.BrokerId,apartment.Location,apartment.Rooms,apartment.Size,DateTime.Now,apartment.BrokerCompany);
            var newBrokerListing = new BrokerListingApartment(newApartment.BrokerId, newApartment.BuildingId);
            await _dbContext.Apartments.AddAsync(newApartment);
            await _dbContext.BrokerListingApartments.AddAsync(newBrokerListing);

            int savedChanges = await _dbContext.SaveChangesAsync();
            if (savedChanges > 0)
            {
                return CreatedAtAction(nameof(GetApartmentById), new { id = newApartment.BuildingId }, newApartment);
            }
            else
            {
                return BadRequest("Failed to save the new apartment.");
            }      
        }
    }
}
