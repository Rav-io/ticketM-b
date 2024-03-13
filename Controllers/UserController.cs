using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ticketmanager.Models;
using ticketmanager.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using BCrypt.Net;


namespace ticketmanager.Controllers
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

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            
            var user = new User
            {
                UserName = model.UserName,
                Password = hashedPassword,
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
            var user = await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserName == model.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1440),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                role = user.Role.Name,
                user = user.UserName
            });
        }

        ///<summary>
        ///Gets all users
        ///</summary>
        /// <param name="user"></param>
        /// <response code="200">Returns all users</response>
        [Authorize]
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

        ///<summary>
        ///Gets all roles
        ///</summary>
        /// <response code="200">Returns all roles</response>
        [Authorize]
        [HttpGet("getroles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            var rolesViewModel = roles.Select(role => new RoleVM
            {
                Id = role.Id,
                Name = role.Name,
            }).ToList();

            return Ok(rolesViewModel);
        }
    }
}
