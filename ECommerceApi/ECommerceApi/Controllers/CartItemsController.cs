namespace ECommerceApi.Controllers
{
    using ECommerceApi.Context;
    using ECommerceApi.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Runtime.CompilerServices;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId) 
        { 
            var user = await _context.Users.FindAsync(userId);

            if (user == null) 
            { 
                return NotFound($"User with id = {userId} not found.");
            }

            var cartItems = await(from s in _context.CartItems.Where(s => s.ClientId == userId)
                                  join p in _context.Products on s.ProductId equals p.Id
                                  select new 
                                  {
                                    Id = s.Id,
                                    Price = s.UnitPrice,
                                    TotalValue = s.TotalValue,
                                    Quantity = s.Quantity,
                                    ProductId = p.Id,
                                    ProductName = p.Name,
                                    ImageUrl = p.ImageUrl,

                                  }).ToListAsync();

            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CartItem cartItem) 
        {
            try
            {
                var shoppingCart = await _context.CartItems.FirstOrDefaultAsync(s =>
                            s.ProductId == cartItem.ProductId &&
                            s.ClientId == cartItem.ClientId);

                if (shoppingCart != null) 
                {
                    shoppingCart.Quantity += cartItem.Quantity;
                    shoppingCart.TotalValue += shoppingCart.UnitPrice * shoppingCart.Quantity;
                }
                else 
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductId);

                    var cart = new CartItem
                    {
                        ClientId = cartItem.ClientId,
                        ProductId = cartItem.ProductId,
                        UnitPrice = cartItem.UnitPrice,
                        Quantity = cartItem.Quantity,
                        TotalValue = (product!.Price) * (cartItem.Quantity),
                    };

                    _context.CartItems.Add(cart);
                }

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int productId, string action) 
        { 
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) 
            { 
                return NotFound("User not found");
            }

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(s =>
                    s.ClientId == user!.Id && s.ProductId == productId);

            if (cartItem != null)
            {
                if (action.ToLower() == "increase")
                {
                    cartItem.Quantity += 1;
                }
                else if (action.ToLower() == "decrease")
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity -= 1;
                    }
                    else
                    {
                        _context.CartItems.Remove(cartItem);
                        await _context.SaveChangesAsync();

                        return Ok();
                    }
                }
                else if (action.ToLower() == "delete")
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid action. Use: 'increase', 'decrease', or 'delete' to perform an action.");
                }

                cartItem.TotalValue = cartItem.UnitPrice * cartItem.Quantity;

                await _context.SaveChangesAsync();

                return Ok($"Operation: {action} successfully executed.");
            }
            else 
            { 
                return NotFound("No item found in the cart.");
            }
        }
    }
}
