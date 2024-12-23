using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryGuysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeliveryGuysController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryGuy>>> GetDeliveryGuys()
        {
            return await _context.DeliveryGuys.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryGuy>> GetDeliveryGuy(int id)
        {
            var deliveryGuy = await _context.DeliveryGuys.FindAsync(id);
            if (deliveryGuy == null)
            {
                return NotFound();
            }
            return deliveryGuy;
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryGuy>> PostDeliveryGuy(DeliveryGuy deliveryGuy)
        {
            _context.DeliveryGuys.Add(deliveryGuy);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeliveryGuy), new { id = deliveryGuy.Id }, deliveryGuy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryGuy(int id, DeliveryGuy deliveryGuy)
        {
            if (id != deliveryGuy.Id)
            {
                return BadRequest();
            }

            _context.Entry(deliveryGuy).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryGuy(int id)
        {
            var deliveryGuy = await _context.DeliveryGuys.FindAsync(id);
            if (deliveryGuy == null)
            {
                return NotFound();
            }

            _context.DeliveryGuys.Remove(deliveryGuy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
