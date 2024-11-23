using ECommerceApi.Context;
using ECommerceApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderDetails(int orderId) 
        { 
            var productDetails = await (from orderDetail in _context.OrderDetails
                                        join order in _context.Orders on orderDetail.OrderId equals order.Id
                                        join product in _context.Products on orderDetail.OrderId equals product.Id
                                        where orderDetail.OrderId == order.Id
                                        select new 
                                        {
                                            Id = orderDetail.Id,
                                            Quantity = orderDetail.Quantity,
                                            SubTotal = orderDetail.TotalValue,
                                            ProductName = product.Name,
                                            ProductImage = product.ImageUrl,
                                            ProductPrice = product.Price,

                                        }).ToListAsync();

            if (productDetails == null || productDetails.Count == 0) 
            { 
                return NotFound("Product details not found."); 
            }

            return Ok(productDetails);
        }

        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrdersByUser(int userId) 
        {
            var orders = await (from order in _context.Orders
                                where order.UserId == userId
                                orderby order.OrderDate descending
                                select new
                                {
                                    Id = order.Id,
                                    TotalOrder = order.TotalValue,
                                    OrderDate = order.OrderDate

                                }).ToListAsync();

            if (orders == null || orders.Count == 0) 
            { 
                return NotFound("No orders were found for the given user."); 
            } 
            
            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Order order) 
        { 
            order.OrderDate = DateTime.Now;

            var cartItems = await _context.CartItems
                .Where(carrinho => carrinho.ClientId == order.UserId)
                .ToListAsync();

            if (cartItems.Count == 0) 
            { 
                return NotFound("There are no items in the cart to create the order."); 
            }

            using (var transaction = await _context.Database.BeginTransactionAsync()) 
            {
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in cartItems) 
                    {
                        var orderDetails = new OrderDetail
                        {
                            Price = item.UnitPrice,
                            TotalValue = item.TotalValue,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            OrderId = order.Id,
                        };

                        _context.OrderDetails.Add(orderDetails);
                    }

                    await _context.SaveChangesAsync();

                    _context.CartItems.RemoveRange(cartItems);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok(new { OrderId = order.Id });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("An error occured while processing your request.");
                }
            }
        }
    }
}
