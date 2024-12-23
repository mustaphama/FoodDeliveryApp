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
    }

    // Endpoint to get most ordered categories
    [HttpGet("GetMostOrderedCategories")]
    public async Task<ActionResult> GetMostOrderedCategories()
    {
        var categories = await _context.Categories
            .GroupJoin(
                _context.OrderItems.Include(oi => oi.FoodItem),
                c => c.Id,
                oi => oi.FoodItem.CategoryId,
                (c, orderItems) => new
                {
                    CategoryId = c.Id,
                    CategoryName = c.CategoryName,
                    OrderCount = orderItems.Sum(oi => oi.Quantity)
                }
            )
            .OrderByDescending(c => c.OrderCount)
            .ToListAsync();

        return Ok(categories);
    }

    // Endpoint to get most ordered products (hot products)
    [HttpGet("GetMostOrderedProducts")]
    public async Task<ActionResult> GetMostOrderedProducts()
    {
        // Get most ordered products by including description and excluding order count
        var products = await _context.OrderItems
            .Include(oi => oi.FoodItem)
            .GroupBy(oi => oi.FoodItemId)
            .Select(g => new
            {
                FoodItemId = g.Key,
                FoodItemName = g.FirstOrDefault().FoodItem.Name,
                Price = g.FirstOrDefault().FoodItem.Price,
                ImageUrl = g.FirstOrDefault().FoodItem.ImageUrl,
                Description = g.FirstOrDefault().FoodItem.Description // Include the description
            })
            .OrderByDescending(p => p.FoodItemName) // You can change the ordering if needed
            .Take(8)
            .ToListAsync();

        return Ok(products);
    }

}
