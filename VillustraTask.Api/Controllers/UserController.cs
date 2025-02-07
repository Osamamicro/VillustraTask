using Microsoft.AspNetCore.Mvc;
using VillustraTask.Api.Helpers;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Userlogin user)
        {
            // TODO: Hash the password before saving
            var result = await _userRepository.InsertUserAsync(user);
            if (result > 0)
            {
                return Ok(new { message = "User registered successfully." });
            }
            return BadRequest(new { message = "Error registering user." });
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Userlogin loginRequest)
        {
            // Retrieve user details
            var user = await _userRepository.GetUserByIdAsync(loginRequest.UserId);
            if (user == null || user.Password != loginRequest.Password)
            {
                // In production, use hashed password verification
                return Unauthorized(new { message = "Invalid credentials." });
            }

            if (user.Islocked)
            {
                return Unauthorized(new { message = "User is locked." });
            }

            // Generate JWT token 
            var token = JwtHelper.GenerateJwtToken(user,
                issuer: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["JwtSettings:Issuer"],
                audience: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["JwtSettings:Audience"],
                secretKey: HttpContext.RequestServices.GetRequiredService<IConfiguration>()["JwtSettings:Secret"],
                expiryInMinutes: int.Parse(HttpContext.RequestServices.GetRequiredService<IConfiguration>()["JwtSettings:ExpiryInMinutes"]));

            return Ok(new { token });
        }
    }
}
