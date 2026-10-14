using Ice_Task2.Model;
using Ice_Task2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _userService.GetAllUsersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updated)
        {
            if (!await _userService.UpdateAsync(id, updated))
                return NotFound(new { message = "User not found." });

            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userService.DeleteAsync(id))
                return NotFound(new { message = "User not found." });

            return NoContent();
        }
    }
}