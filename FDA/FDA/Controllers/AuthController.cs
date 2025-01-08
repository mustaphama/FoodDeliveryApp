using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FDA.Data;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Assuming FDAContext is your database context
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>(); // Password hasher

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Check if user already exists by email
            var userExists = _context.Users.Any(u => u.Email == request.Email);
            if (userExists)
            {
                return BadRequest(new { message = "User already exists" });
            }

            // Hash the user's password
            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            // Create new user object with hashed password
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                ProfilePictureUrl = request.ProfilePictureUrl,
                CreatedAt = DateTime.Now
            };

            // Add user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully!" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Check if user exists by email
            var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password!" });
            }

            // Verify the password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Invalid email or password!" });
            }

            // Return the UserId to the client upon successful login
            return Ok(new
            {
                message = "Login successful!",
                userId = user.Id // Add userId in the response
            });
        }
        [HttpPut("modifyPassword")]
        public async Task<IActionResult> ModifyPassword([FromBody] ModifyPasswordRequest request)
        {
            // Validate the user's old password
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) return NotFound("User not found.");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, request.OldPassword);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest("Incorrect old password.");
            }

            // Update to the new password
            user.PasswordHash = _passwordHasher.HashPassword(null, request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Password updated successfully.");
        }
        [HttpPut("modifyEmail")]
        public async Task<IActionResult> ModifyEmail([FromBody] ModifyEmailRequest request)
        {
            // Validate the user's password
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) return NotFound("User not found.");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest("Incorrect password.");
            }

            // Check if the new email is already taken
            if (_context.Users.Any(u => u.Email == request.NewEmail))
            {
                return BadRequest("The email is already in use.");
            }

            // Update to the new email
            user.Email = request.NewEmail;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Email updated successfully.");
        }
        // In UserController.cs
        [HttpPut("modifyName")]
        public async Task<IActionResult> ModifyName([FromBody] ModifyNameRequest request)
        {
            // Validate the user's password
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null) return NotFound("User not found.");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, request.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest("Incorrect password.");
            }           

            // Update the user's name
            user.Name = request.NewName;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Name updated successfully.");
        }

    }
}
