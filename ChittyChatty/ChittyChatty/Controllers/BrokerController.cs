using ChittyChatty.Data;
using ChittyChatty.Domain.Entites;
using ChittyChatty.Dtos;
using ChittyChatty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ChittyChatty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrokerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BrokerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BrokerRm>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrokers()
        {
            var brokerList = await _context.Brokers
                .Select(b => new BrokerRm
                {
                    BrokerId = b.BrokerId,
                    FirstName = b.FirstName,
                    Surname = b.Surname,
                    BrokerCompany = b.BrokerCompany,
                    LastUpdate = b.LastUpdate
                })
                .ToListAsync();

            if(brokerList == null)
                return NotFound();
            return Ok(brokerList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BrokerRm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrokerById(Guid id)
        {
            var broker = await _context.Brokers
                .Where(b => b.BrokerId == id)
                .Select(broker => new BrokerRm
                {
                    BrokerId = broker.BrokerId,
                    FirstName = broker.FirstName,
                    Surname = broker.Surname,
                    BrokerCompany = broker.BrokerCompany,
                    LastUpdate = broker.LastUpdate
                })
                .FirstOrDefaultAsync();

            if(broker == null)
                return NotFound();

            return Ok(broker);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBrokerById(Guid id)
        {
            var broker = await _context.Brokers.FindAsync(id);

            if(broker is null)
                return NotFound();

            _context.Brokers.Remove(broker);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostNewBroker(BrokerDto broker)
        {
            var newBroker = new Broker(broker.FirstName,broker.Surname,broker.BrokerCompany,DateTime.Now);
            await _context.Brokers.AddAsync(newBroker);

            var savedChanges = await _context.SaveChangesAsync();
            var newBrokerRm = new BrokerRm
            {
                BrokerId = newBroker.BrokerId,
                FirstName = newBroker.FirstName,
                Surname = newBroker.Surname,
                BrokerCompany = newBroker.BrokerCompany,
                LastUpdate = newBroker.LastUpdate
            };

            if(savedChanges > 0)
            {
                return CreatedAtAction(nameof(GetBrokerById), new { id = newBroker.BrokerId }, newBrokerRm);
            }
            else
            {
                return BadRequest("Could not save the new broker");
            }
        }
    }
}
