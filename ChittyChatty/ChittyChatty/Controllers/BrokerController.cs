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
            var brokerList = await _context.Brokers.ToListAsync();

            if(brokerList == null)
                return NotFound();
            return Ok(brokerList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BrokerRm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrokerById(Guid id)
        {
            var broker = await _context.Brokers.FindAsync(id);

            if(broker == null)
                return NotFound();

            return Ok(broker);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostNewBroker(BrokerDto broker)
        {
            var newBroker = new Broker(broker.FirstName,broker.Surname,broker.BrokerCompany,broker.LastUpdate);
            await _context.Brokers.AddAsync(newBroker);

            var savedChanges = await _context.SaveChangesAsync();
            if(savedChanges > 0)
            {
                return CreatedAtAction(nameof(GetBrokerById), new { id = newBroker.BrokerId }, newBroker);
            }
            else
            {
                return BadRequest("Could not save the new broker");
            }
        }
    }
}
