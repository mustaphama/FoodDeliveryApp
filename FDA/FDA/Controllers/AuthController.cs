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


    }

    // Request model for registration
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    // Request model for login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
