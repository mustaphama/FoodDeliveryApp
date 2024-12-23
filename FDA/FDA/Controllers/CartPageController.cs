using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartPageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems()
        {
            return await _context.FoodItems.ToListAsync();
        }
        [HttpPost("GetByIds")]
        public async Task<ActionResult<IEnumerable<FoodItemWithRestaurantDto>>> GetFoodItemsWithRestaurants([FromBody] List<int> ids)
        {
            var foodItemsWithRestaurants = await _context.FoodItems
                .Where(fi => ids.Contains(fi.Id))
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
                .ToListAsync();

            return Ok(foodItemsWithRestaurants);
        }


    }
}