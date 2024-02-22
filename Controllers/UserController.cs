using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projekt_zespolowy.Models;
using projekt_zespolowy.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace projekt_zespolowy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        private const string SecretKey = "VerySecretKeyTbh";
        private readonly SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        ///<summary>
        ///User sign up
        ///</summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "userName": "user1",
        ///        "password": "password123",
        ///        "role": "user"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Username already exists</response>
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
            {
                return BadRequest("Username is already taken.");
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);

            if (role == null)
            {
                role = new Role { Name = model.Role };
                _context.Roles.Add(role);
            }
            
            var user = new User
            {
                UserName = model.UserName,
                Password = model.Password,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }

        ///<summary>
        ///User log in
        ///</summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "userName": "user1",
        ///        "password": "password123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">User logged in successfully</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginVM model)
        {
            // Include the Role in the query
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserName == model.UserName);

            if (user == null || model.Password != user.Password)
            {
                return Unauthorized("Invalid credentials");
            }

            // Create claims for the JWT token, including the user's role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name) // Assuming the Role property is not null
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            // Return the token, expiration, and user's role in the response
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                role = user.Role.Name // Include the user's role in the response
            });
        }

        ///<summary>
        ///Gets all users
        ///</summary>
        /// <param name="user"></param>
        /// <response code="200">Returns all users</response>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userViewModels = users.Select(user => new UserNameVM
            {
                Id = user.Id,
                UserName = user.UserName,
            }).ToList();

            return Ok(userViewModels);
        }
    }
}
