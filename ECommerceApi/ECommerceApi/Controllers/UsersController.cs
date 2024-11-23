namespace ECommerceApi.Controllers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using ECommerceApi.Context;
    using ECommerceApi.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] User user) 
        { 
            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userExists != null) 
            {
                return BadRequest("A user with that email already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] User user) 
        { 
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => 
                              u.Email == user.Email && u.Password == user.Password);

            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new ObjectResult(new
            {
                access_token = jwt,
                token_type = "bearer",
                user_id = currentUser.Id,
                user_name = currentUser.Name,
            });
        }

        [Authorize]
        [HttpPost("uploadphoto")]
        public async Task<IActionResult> UploadUserPhoto(IFormFile image) 
        { 
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) 
            { 
                return NotFound("User not found");
            }

            if (image != null) 
            { 
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine("wwwroot/userimages", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create)) 
                { 
                    await image.CopyToAsync(stream);
                }

                user.ImageUrl = "/userimages/" + uniqueFileName;

                await _context.SaveChangesAsync();

                return Ok("Image successfully sent.");
            }

            return BadRequest("No image was sent.");
        }

        [Authorize]
        [HttpGet("profileimage")]
        public async Task<IActionResult> UserProfileImage() 
        { 
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; 
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) 
            { 
                return NotFound("User not found");
            }

            var profileImage = await _context.Users
                .Where(u => u.Email == userEmail)
                .Select(u => new 
                { 
                    u.ImageUrl,

                }).SingleOrDefaultAsync();

            return Ok(profileImage);
        }
    }
}
