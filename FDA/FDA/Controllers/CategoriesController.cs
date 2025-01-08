using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpGet("{categoryId}/food-items")]
        public async Task<IActionResult> GetFoodItemsByCategory(int categoryId)
        {
            var foodItems = await _context.FoodItems
                .Where(fi => fi.CategoryId == categoryId)
                .Include(fi => fi.Menu) // Include the Menu table
                .ThenInclude(m => m.Restaurant) // Include the Restaurant table
                .Select(fi => new
                {
                    FoodItemId = fi.Id,
                    FoodName = fi.Name,
                    Description = fi.Description,
                    Price = fi.Price,
                    ImageUrl = fi.ImageUrl,
                    IsAvailable = fi.IsAvailable,
                    Restaurant = new
                    {
                        RestaurantId = fi.Menu.Restaurant.Id,
                        RestaurantName = fi.Menu.Restaurant.Name,
                        Location = fi.Menu.Restaurant.Location,
                        LogoUrl = fi.Menu.Restaurant.LogoUrl
                    }
                })
                .ToListAsync();

            if (!foodItems.Any())
            {
                return NotFound(new { Message = "No food items found for the specified category." });
            }

            return Ok(foodItems);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
