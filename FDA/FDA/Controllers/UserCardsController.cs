using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FDA.Models;
using FDA.Data;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCards>>> GetUserCards()
        {
            return await _context.UserCards
                .Include(uc => uc.PromotionCard)
                .ToListAsync();
        }

        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<UserCards>>> GetUserCardsByUserId(int userId)
        {
            var userCards = await _context.UserCards
                .Where(uc => uc.Id_Users == userId)
                .Include(uc => uc.PromotionCard)
                .ToListAsync();

            if (userCards == null || userCards.Count == 0)
            {
                return NotFound(new { Message = $"No cards found for User ID {userId}." });
            }

            return Ok(userCards);
}

                // GET: api/UserCards/{id}
                [HttpGet("{id}")]
        public async Task<ActionResult<UserCards>> GetUserCard(int id)
        {
            var userCard = await _context.UserCards
                .Include(uc => uc.PromotionCard)
                .FirstOrDefaultAsync(uc => uc.Id == id);

            if (userCard == null)
            {
                return NotFound();
            }

            return userCard;
        }

        // POST: api/UserCards
        [HttpPost]
        public async Task<ActionResult<UserCards>> PostUserCard(UserCards userCard)
        {
            _context.UserCards.Add(userCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserCard), new { id = userCard.Id }, userCard);
        }

        // PUT: api/UserCards/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCard(int id, UserCards userCard)
        {
            if (id != userCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(userCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/UserCards/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCard(int id)
        {
            var userCard = await _context.UserCards.FindAsync(id);
            if (userCard == null)
            {
                return NotFound();
            }

            _context.UserCards.Remove(userCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCardExists(int id)
        {
            return _context.UserCards.Any(e => e.Id == id);
        }
    }
}
