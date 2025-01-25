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
                    Id_Restaurants = fi.Menu.Restaurant.Id, // Include Id_Restaurants
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantAddress = fi.Menu.Restaurant.Address
                })
                .ToListAsync();

            return Ok(foodItemsWithRestaurants);
        }


    }
}