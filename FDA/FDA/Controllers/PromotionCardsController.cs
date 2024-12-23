using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionCardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromotionCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionCard>>> GetPromotionCards()
        {
            return await _context.PromotionCards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionCard>> GetPromotionCard(int id)
        {
            var promotionCard = await _context.PromotionCards.FindAsync(id);
            if (promotionCard == null)
            {
                return NotFound();
            }
            return promotionCard;
        }

        [HttpPost]
        public async Task<ActionResult<PromotionCard>> PostPromotionCard(PromotionCard promotionCard)
        {
            _context.PromotionCards.Add(promotionCard);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPromotionCard), new { id = promotionCard.Id }, promotionCard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotionCard(int id, PromotionCard promotionCard)
        {
            if (id != promotionCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(promotionCard).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotionCard(int id)
        {
            var promotionCard = await _context.PromotionCards.FindAsync(id);
            if (promotionCard == null)
            {
                return NotFound();
            }

            _context.PromotionCards.Remove(promotionCard);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
