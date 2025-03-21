using Application.Services.Auth;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Threading.Tasks;
using Infrastructure.Database;
using Infrastructure.Log;

namespace Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly MongoDbContext _dbContext;
		private readonly ILogger _logger;

        public AuthController(IAuthService authService, MongoDbContext dbContext, ILogger logger)
        {
            _authService = authService;
            _dbContext = dbContext;
			_logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
			_logger.Log("Attempting user login for email:", loginDto.Email);
            var user = await _dbContext.GetCollection<User>("Users")
                                       .Find(u => u.Email == loginDto.Email)
                                       .FirstOrDefaultAsync();

            if (user == null || !_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
			_logger.Log("Attempting user signup for email:", registerDto.Email);
            var existingUser = await _dbContext.GetCollection<User>("Users")
                                               .Find(u => u.Email == registerDto.Email)
                                               .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return Conflict(new { message = "User already exists" });
            }

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = _authService.HashPassword(registerDto.Password),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _dbContext.GetCollection<User>("Users").InsertOneAsync(newUser);
            var token = _authService.GenerateToken(newUser);

            return Created("api/auth/register", new { token });
        }
    }

    public record UserLoginDto(string Email, string Password);
    public record UserRegisterDto(string FirstName, string LastName, string Email, string Password);
}

