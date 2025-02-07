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
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
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

            // Do not return password in response
            return Ok(new
            {
                user.UserId,
                user.FullName,
                user.DesignationId,
                user.ProfilePicture
            });
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Userlogin user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest(new { message = "Password is required." });
            }

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
            var user = await _userRepository.AuthenticateUserAsync(loginRequest.UserId, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            if (user.Islocked)
            {
                return Unauthorized(new { message = "User is locked." });
            }

            // Generate JWT using _configuration
            var token = JwtHelper.GenerateJwtToken(user, _configuration);

            return Ok(new { token });
        }
    }
}
