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
        public async Task<ActionResult> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems
                .Where(fi => fi.Id == id)
                .Select(fi => new
                {
                    fi.Id,
                    fi.Name,
                    fi.Description,
                    fi.Price,
                    fi.Ratings,
                    fi.NumOfReviews,
                    fi.IsAvailable,
                    fi.CreatedAt,
                    Restaurant = new
                    {
                        fi.Menu.Restaurant.Id,
                        fi.Menu.Restaurant.Name,
                        fi.Menu.Restaurant.Email,
                        fi.Menu.Restaurant.PhoneNumber,
                        fi.Menu.Restaurant.Address,
                        fi.Menu.Restaurant.City,
                        fi.Menu.Restaurant.Country,
                        fi.Menu.Restaurant.CreatedAt
                    }
                })
                .FirstOrDefaultAsync();

            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(foodItem);
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
                .Select(fi => new
                {
                    Id = fi.Id,
                    Name = fi.Name,
                    Description = fi.Description,
                    Price = fi.Price,
                    IsAvailable = fi.IsAvailable,
                    Ratings = fi.Ratings,
                    NumOfReviews = fi.NumOfReviews,
                    Restaurant = new
                    {
                        Id = fi.Menu.Restaurant.Id,
                        Name = fi.Menu.Restaurant.Name,
                        Address = fi.Menu.Restaurant.Address,
                    }
                })
                .ToListAsync();

            return Ok(results);
        }
        [HttpPost("submit-rating")]
        public async Task<IActionResult> SubmitRating([FromBody] RatingRequest ratingRequest)
        {
            // Get the food item
            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(f => f.Id == ratingRequest.FoodItemId);

            if (foodItem == null)
            {
                return NotFound("Food item not found.");
            }

            // Check if the user has ordered the food item before
            var hasOrdered = await _context.OrderItems
                .AnyAsync(oi => oi.Order.Id_Users == ratingRequest.UserId && oi.Id_FoodItems == ratingRequest.FoodItemId);

            if (!hasOrdered)
            {
                return BadRequest("You can only rate products you have previously ordered.");
            }

            // Get the current rating and number of reviews for the food item
            var oldRating = foodItem.Ratings;
            var oldNumberOfReviews = foodItem.NumOfReviews;

            var newRating = ratingRequest.Rating;

            // Calculate the new rating using the formula
            var updatedRating = ((oldRating * oldNumberOfReviews) + newRating) / (oldNumberOfReviews + 1);

            // Update the food item's rating and increment the number of reviews
            foodItem.Ratings = updatedRating;
            foodItem.NumOfReviews += 1;

            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Thank you for your rating!" });
        }

        [HttpGet("menus/{menuId}/fooditems")]
        public async Task<IActionResult> GetFoodItemsByMenuId(int menuId)
        {
            var foodItems = await _context.FoodItems
                                            .Where(fi => fi.Id_Menus == menuId)
                                            .ToListAsync();

            if (!foodItems.Any())
                return NotFound("No food items found for this menu.");

            return Ok(foodItems);
        }
        [HttpGet("{id}/recommendations")]
        public async Task<ActionResult> GetRecommendations(int id)
        {
            // Fetch the main food item
            var mainFoodItem = await _context.FoodItems
                .Include(fi => fi.Menu.Restaurant) // Include Restaurant info
                .Include(fi => fi.Category) // Assuming there's a Category navigation property
                .FirstOrDefaultAsync(fi => fi.Id == id);

            if (mainFoodItem == null)
            {
                return NotFound("Food item not found.");
            }

            // Fetch recommended items
            var recommendations = await _context.FoodItems
                .Where(fi =>
                    fi.Id != id && // Exclude the main food item
                    fi.IsAvailable && // Only available items
                    (
                        fi.Menu.Restaurant.Id == mainFoodItem.Menu.Restaurant.Id || // Same restaurant
                        fi.Category.Id == mainFoodItem.Category.Id // Same category
                    ))
                .OrderByDescending(fi => _context.OrderItems.Count(oi => oi.Id_FoodItems == fi.Id)) // Most ordered items
                .ThenByDescending(fi => fi.Ratings) // Further prioritize by ratings
                .Take(8) // Limit to 8 recommendations
                .Select(fi => new
                {
                    fi.Id,
                    fi.Name,
                    fi.Description,
                    fi.Price,
                    fi.Ratings,
                    fi.NumOfReviews,
                    Restaurant = new
                    {
                        fi.Menu.Restaurant.Id,
                        fi.Menu.Restaurant.Name,
                        fi.Menu.Restaurant.Address
                    }
                })
                .ToListAsync();

            return Ok(recommendations);
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
                    Id_Restaurants = fi.Menu.Restaurant.Id, // Include Id_Restaurants
                    RestaurantName = fi.Menu.Restaurant.Name,
                    RestaurantAddress = fi.Menu.Restaurant.Address
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
