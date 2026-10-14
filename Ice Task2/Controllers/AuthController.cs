using Ice_Task2.Model;
using Ice_Task2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null) return BadRequest(new { message = "Invalid request." });

            if (await _userService.GetUserByEmailAsync(user.Email) != null)
                return Conflict(new { message = "Email already registered." });

            var created = await _userService.RegisterAsync(user);
            return Ok(new { message = "Registration successful.", user = created });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(new { message = "Login successful.", user });
        }
    } 
}
