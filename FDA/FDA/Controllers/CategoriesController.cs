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

        //[HttpGet("{Id_Categories}/food-items")]
        //public async Task<IActionResult> GetFoodItemsByCategory(int Id_Categories)
        //{
        //    var foodItems = await _context.FoodItems
        //        .Where(fi => fi.Id_Categories == Id_Categories)
        //        .Include(fi => fi.Menu) // Include the Menu table
        //        .ThenInclude(m => m.Restaurant) // Include the Restaurant table
        //        .Select(fi => new
        //        {
        //            Id_FoodItems = fi.Id,
        //            FoodName = fi.Name,
        //            Description = fi.Description,
        //            Price = fi.Price,
        //            IsAvailable = fi.IsAvailable,
        //            Ratings = fi.Ratings,
        //            NumOfReviews = fi.NumOfReviews,
        //            Restaurant = new
        //            {
        //                Id_Restaurants = fi.Menu.Restaurant.Id,
        //                RestaurantName = fi.Menu.Restaurant.Name,
        //                Address = fi.Menu.Restaurant.Address,
        //            }
        //        })
        //        .ToListAsync();

        //    if (!foodItems.Any())
        //    {
        //        return NotFound(new { Message = "No food items found for the specified category." });
        //    }

        //    return Ok(foodItems);
        //}

        [HttpGet("{Id_Categories}/food-items")]
        public async Task<IActionResult> GetFoodItemsByCategory(int Id_Categories, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page number and page size must be greater than 0." });
            }

            var query = _context.FoodItems
                .Where(fi => fi.Id_Categories == Id_Categories)
                .Include(fi => fi.Menu) // Include the Menu table
                .ThenInclude(m => m.Restaurant) // Include the Restaurant table
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
                        Id= fi.Menu.Restaurant.Id,
                        Name = fi.Menu.Restaurant.Name,
                        Address = fi.Menu.Restaurant.Address,
                    }
                });

            // Apply pagination
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0)
            {
                return NotFound(new { Message = "Page number exceeds total pages available." });
            }

            var foodItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return paginated response
            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Items = foodItems
            };

            if (!foodItems.Any())
            {
                return NotFound(new { Message = "No food items found for the specified category." });
            }

            return Ok(response);
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
