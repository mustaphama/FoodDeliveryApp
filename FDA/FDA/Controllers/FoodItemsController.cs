using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
        {
            return await _context.FoodItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return foodItem;
        }

        [HttpPost]
        public async Task<ActionResult<FoodItem>> PostFoodItem(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFoodItem), new { id = foodItem.Id }, foodItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItem(int id, FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchFoodItems(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty.");

            var results = await _context.FoodItems
                .Where(f => f.Name.Contains(query) || f.Description.Contains(query))
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.ImageUrl,
                    RestaurantName = f.Menu.Restaurant.Name
                })
                .ToListAsync();

            return Ok(results);
        }


        [HttpGet("FoodItemWithRestaurant/{id}")]
        public async Task<ActionResult<FoodItemWithRestaurantDto>> GetFoodItemWithRestaurant(int id)
        {
            var foodItemWithRestaurant = await _context.FoodItems
                .Where(fi => fi.Id == id)
                .Select(fi => new FoodItemWithRestaurantDto
                {
                    Id = fi.Id,
                    Name = fi.Name,
                    Description = fi.Description,
                    Price = fi.Price,
                    ImageUrl = fi.ImageUrl,
                    RestaurantId = fi.Menu.Restaurant.Id, // Include RestaurantId
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantLocation = fi.Menu.Restaurant.Location
                })
                .FirstOrDefaultAsync();

            if (foodItemWithRestaurant == null)
            {
                return NotFound();
            }

            return Ok(foodItemWithRestaurant);
        }
    }

}
