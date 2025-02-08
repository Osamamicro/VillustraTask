using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userId = User.Identity.Name;
            return Ok(new { userId });
        }

        /// <summary>
        /// Get user details by ID (Authenticated)
        /// </summary>
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User not found: {userId}");
                return NotFound(new { message = "User not found." });
            }

            return Ok(new
            {
                user.UserId,
                user.FullName,
                user.DesignationId,
                user.ProfilePicture
            });
        }
        // GetUsers
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Userlogin
            {
                UserId = registerRequest.UserId,
                Password = registerRequest.Password,
                FullName = registerRequest.FullName,
                DesignationId = registerRequest.DesignationId,
                ProfilePicture = registerRequest.ProfilePicture,
                CreatedBy = registerRequest.UserId
            };

            var result = await _userRepository.InsertUserAsync(user);

            if (result > 0)
            {
                _logger.LogInformation($"New user registered: {registerRequest.UserId}");
                return Ok(new { message = "User registered successfully." });
            }

            _logger.LogWarning($"User registration failed for: {registerRequest.UserId}");
            return BadRequest(new { message = "Error registering user. The user might already exist or an internal error occurred." });
        }

        /// <summary>
        /// Authenticate user & generate JWT token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.AuthenticateUserAsync(loginRequest.UserId, loginRequest.Password);
            if (user == null)
            {
                _logger.LogWarning($"Invalid login attempt: {loginRequest.UserId}");
                return Unauthorized(new { message = "Invalid credentials." });
            }

            if (user.IsLocked)
            {
                _logger.LogWarning($"Locked user attempted login: {loginRequest.UserId}");
                return Unauthorized(new { message = "User is locked." });
            }

            var token = JwtHelper.GenerateJwtToken(user, _configuration);
            return Ok(new { token });
        }
    }
}
