using FDA.Data;
using FDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace FDA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] CartRequest cartRequest)
        {
            if (cartRequest == null || !cartRequest.CartItems.Any())
                return BadRequest("Cart is empty or invalid.");

            try
            {
                // Find the first available delivery guy with a car
                var deliveryGuy = await _context.DeliveryGuys
                    .Where(dg => dg.Availability == true)
                    .OrderBy(dg => dg.VehiculeType == "Car" ? 0 : 1) // Prioritize cars
                    .FirstOrDefaultAsync();

                if (deliveryGuy == null)
                    return StatusCode(500, "No delivery guys are currently available.");
                Debug.WriteLine(cartRequest.Id_Users);
                // Create new order
                var order = new Order
                {
                    Id_Users = cartRequest.Id_Users, // Retrieve from Preferences or request
                    TotalAmount = cartRequest.CartItems.Sum(ci => ci.Price * ci.Quantity),
                    OrderStatus = "Pending",
                    Id_DeliveryGuys = deliveryGuy.Id, // Assign the selected delivery guy
                    Id_PromotionCards = null, // Placeholder for now
                    CreatedAt = DateTime.Now,
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add OrderItems
                foreach (var item in cartRequest.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Id_Orders = order.Id,
                        Id_FoodItems = item.Id_FoodItems,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync();

                // Update delivery guy's availability
                deliveryGuy.Availability = false;
                _context.DeliveryGuys.Update(deliveryGuy);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Order placed successfully.", Id_Orders = order.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get the status of an order by its ID
        [HttpGet("{Id_Orders}/status")]
        public async Task<IActionResult> GetOrderStatus(int Id_Orders)
        {
            var order = await _context.Orders
                .Where(o => o.Id == Id_Orders)
                .Select(o => o.OrderStatus)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            return Ok(order); // Return the status of the order
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
