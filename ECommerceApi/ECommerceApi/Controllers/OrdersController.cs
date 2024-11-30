namespace ECommerceApi.Controllers
{
    using ECommerceApi.Context;
    using ECommerceApi.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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
            var orderDetails = await _context.OrderDetails
                                        .AsNoTracking()
                                        .Where(od => od.OrderId == orderId)
                                        .Select(orderDetail => new
                                        {
                                            Id = orderDetail.Id,
                                            Quantity = orderDetail.Quantity,
                                            SubTotal = orderDetail.TotalValue,
                                            ProductName = orderDetail.Product!.Name,
                                            ProductImage = orderDetail.Product.ImageUrl,
                                            ProductPrice = orderDetail.Product.Price,

                                        }).ToListAsync();

            if (!orderDetails.Any()) 
            { 
                return NotFound("Order details not found."); 
            }

            return Ok(orderDetails);
        }

        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrdersByUser(int userId) 
        {
            var orders = await _context.Orders
                                .AsNoTracking()
                                .Where (o => o.UserId == userId)
                                .OrderByDescending(o => o.OrderDate)
                                .Select(orders => new
                                {
                                    Id = orders.Id,
                                    TotalOrder = orders.TotalValue,
                                    OrderDate = orders.OrderDate

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
