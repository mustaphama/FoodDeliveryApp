using FDA.Data;
using FDA.Models; // Adjust to your actual namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
        _context.ChangeTracker.Clear();
    }

    // Endpoint to get most ordered categories
    [HttpGet("GetMostOrderedCategories")]
    public async Task<ActionResult> GetMostOrderedCategories()
    {
        _context.ChangeTracker.Clear();
        var categories = await _context.Categories
            .GroupJoin(
                _context.OrderItems.Include(oi => oi.FoodItem),
                c => c.Id,
                oi => oi.FoodItem.Id_Categories,
                (c, orderItems) => new
                {
                    Id_Categories = c.Id,
                    CategoryName = c.CategoryName,
                    OrderCount = orderItems.Sum(oi => oi.Quantity)
                }
            )
            .OrderByDescending(c => c.OrderCount)
            .ToListAsync();

        return Ok(categories);
    }

    //// Endpoint to get most ordered products (hot products)
    //[HttpGet("GetMostOrderedProducts")]
    //public async Task<ActionResult> GetMostOrderedProducts()
    //{
    //    // Get most ordered products by including description and excluding order count
    //    var products = await _context.OrderItems
    //.Include(oi => oi.FoodItem)
    //.GroupBy(oi => oi.Id_FoodItems) // Make sure this is the new field
    //.Select(g => new
    //{
    //    Id_FoodItems = g.Key,
    //    FoodItemName = g.FirstOrDefault().FoodItem.Name,
    //    Price = g.FirstOrDefault().FoodItem.Price,
    //    Description = g.FirstOrDefault().FoodItem.Description // Include the description
    //})
    //.OrderByDescending(p => p.FoodItemName) // You can change the ordering if needed
    //.Take(8)
    //.ToListAsync();

    //    return Ok(products);
    //}

    [HttpGet("GetMostOrderedProducts")]
    public async Task<IActionResult> GetFoodItemsByUserLocation(int userId)
    {
        // Get the user's city and country
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.City, u.Country })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Get food items from restaurants in the same city and country as the user
        var foodItems = await _context.FoodItems
            .Include(fi => fi.Menu)
                .ThenInclude(m => m.Restaurant) // Include restaurant details
            .Where(fi => fi.Menu.Restaurant.City == user.City && fi.Menu.Restaurant.Country == user.Country)
            .Select(fi => new
            {
                Id = fi.Id,
                Name = fi.Name,
                Description = fi.Description,
                Price = fi.Price,
                Ratings = fi.Ratings,
                NumOfReviews = fi.NumOfReviews,
                IsAvailable = fi.IsAvailable,
                Restaurant = new
                {
                    RestaurantId = fi.Menu.Restaurant.Id,
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantEmail = fi.Menu.Restaurant.Email,
                    RestaurantPhoneNumber = fi.Menu.Restaurant.PhoneNumber,
                    RestaurantAddress = fi.Menu.Restaurant.Address,
                    RestaurantCity = fi.Menu.Restaurant.City,
                    RestaurantCountry = fi.Menu.Restaurant.Country
                }
            })
            .Take(8)
            .ToListAsync();

        if (foodItems.Count == 0)
        {
            return NotFound("No food items found for the user's location.");
        }

        return Ok(foodItems);
    }
    [HttpGet("GetHighestRatedProducts")]
    public async Task<IActionResult> GetHighestRatedFoodItems(int userId)
    {
        // Get the user's city and country
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.City, u.Country })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }
        // Get the top 8 food items sorted by ratings in descending order
        var foodItems = await _context.FoodItems
            .Include(fi => fi.Menu)
                .ThenInclude(m => m.Restaurant) // Include restaurant details
                .Where(fi => fi.Menu.Restaurant.City == user.City && fi.Menu.Restaurant.Country == user.Country)
            .OrderByDescending(fi => fi.Ratings) // Order by ratings in descending order
            .ThenByDescending(fi => fi.NumOfReviews) // Optionally, order by number of reviews for better accuracy
            .Select(fi => new
            {
                Id = fi.Id,
                Name = fi.Name,
                Description = fi.Description,
                Price = fi.Price,
                Ratings = fi.Ratings,
                NumOfReviews = fi.NumOfReviews,
                IsAvailable = fi.IsAvailable,
                Restaurant = new
                {
                    RestaurantId = fi.Menu.Restaurant.Id,
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantEmail = fi.Menu.Restaurant.Email,
                    RestaurantPhoneNumber = fi.Menu.Restaurant.PhoneNumber,
                    RestaurantAddress = fi.Menu.Restaurant.Address,
                    RestaurantCity = fi.Menu.Restaurant.City,
                    RestaurantCountry = fi.Menu.Restaurant.Country
                }
            })
            .Take(8)
            .ToListAsync();

        if (foodItems.Count == 0)
        {
            return NotFound("No highly rated food items found.");
        }

        return Ok(foodItems);
    }
    [HttpGet("GetNewestFoodItems")]
    public async Task<IActionResult> GetNewestFoodItems(int userId)
    {
        // Get the user's city and country
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.City, u.Country })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }
        // Get the top 8 food items sorted by the creation date (assuming there is a 'CreatedDate' field)
        var foodItems = await _context.FoodItems
            .Include(fi => fi.Menu)
                .ThenInclude(m => m.Restaurant) // Include restaurant details
                .Where(fi => fi.Menu.Restaurant.City == user.City && fi.Menu.Restaurant.Country == user.Country)
            .OrderByDescending(fi => fi.CreatedAt) // Order by the creation date in descending order (most recent first)
            .Select(fi => new
            {
                Id = fi.Id,
                Name = fi.Name,
                Description = fi.Description,
                Price = fi.Price,
                Ratings = fi.Ratings,
                NumOfReviews = fi.NumOfReviews,
                IsAvailable = fi.IsAvailable,
                Restaurant = new
                {
                    RestaurantId = fi.Menu.Restaurant.Id,
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantEmail = fi.Menu.Restaurant.Email,
                    RestaurantPhoneNumber = fi.Menu.Restaurant.PhoneNumber,
                    RestaurantAddress = fi.Menu.Restaurant.Address,
                    RestaurantCity = fi.Menu.Restaurant.City,
                    RestaurantCountry = fi.Menu.Restaurant.Country
                }
            })
            .Take(8)
            .ToListAsync();

        if (foodItems.Count == 0)
        {
            return NotFound("No recently added food items found.");
        }

        return Ok(foodItems);
    }
    [HttpGet("GetCheapestFoodItems")]
    public async Task<IActionResult> GetCheapestFoodItems(int userId)
    {
        // Get the user's city and country
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.City, u.Country })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }
        // Get the top 8 food items sorted by price in ascending order (cheapest first)
        var foodItems = await _context.FoodItems
            .Include(fi => fi.Menu)
                .ThenInclude(m => m.Restaurant) // Include restaurant details
                .Where(fi => fi.Menu.Restaurant.City == user.City && fi.Menu.Restaurant.Country == user.Country)
            .OrderBy(fi => fi.Price) // Order by price in ascending order
            .Select(fi => new
            {
                Id = fi.Id,
                Name = fi.Name,
                Description = fi.Description,
                Price = fi.Price,
                Ratings = fi.Ratings,
                NumOfReviews = fi.NumOfReviews,
                IsAvailable = fi.IsAvailable,
                Restaurant = new
                {
                    RestaurantId = fi.Menu.Restaurant.Id,
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantEmail = fi.Menu.Restaurant.Email,
                    RestaurantPhoneNumber = fi.Menu.Restaurant.PhoneNumber,
                    RestaurantAddress = fi.Menu.Restaurant.Address,
                    RestaurantCity = fi.Menu.Restaurant.City,
                    RestaurantCountry = fi.Menu.Restaurant.Country
                }
            })
            .Take(8)
            .ToListAsync();

        if (foodItems.Count == 0)
        {
            return NotFound("No cheap food items found.");
        }

        return Ok(foodItems);
    }




}
