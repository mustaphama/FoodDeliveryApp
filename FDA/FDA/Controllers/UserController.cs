using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}/address")]
        public async Task<IActionResult> UpdateUserAddress(int id, [FromBody] UpdateAddressRequest request)
        {
            if (id <= 0 || string.IsNullOrEmpty(request.Address))
            {
                return BadRequest("Invalid input.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Address = request.Address;
            _context.Entry(user).Property(u => u.Address).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("A concurrency error occurred while updating the address.");
            }

            return NoContent();
        }
        [HttpGet("getEmail/{Id_Users}")]
        public async Task<IActionResult> GetEmail(int Id_Users)
        {
            var user = await _context.Users.FindAsync(Id_Users);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user.Email);
        }


        [HttpPut("{id}/phoneNumber")]
        public async Task<IActionResult> UpdateUserPhone(int id, [FromBody] UpdatePhoneRequest request)
        {
            if (id <= 0 || string.IsNullOrEmpty(request.Phone))
            {
                return BadRequest("Invalid input.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.PhoneNumber = request.Phone;
            _context.Entry(user).Property(u => u.PhoneNumber).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("A concurrency error occurred while updating the address.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
