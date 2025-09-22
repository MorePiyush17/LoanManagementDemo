using LoanManagement.Models;
using LoanManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // ✅ GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserProfileAsync(id);
            if (user == null) return NotFound(new { Message = "User not found" });
            return Ok(user);
        }

        // ✅ POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(user, user.Role.ToString());
            if (!result)
                return BadRequest(new { Message = "User registration failed" });

            return Ok(new { Message = "User registered successfully" });
        }

        // ✅ POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password" });

            return Ok(user);
        }

        // ✅ PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId) return BadRequest();

            var result = await _userService.UpdateUserProfileAsync(user);
            if (!result) return NotFound(new { Message = "User not found or update failed" });

            return Ok(new { Message = "User updated successfully" });
        }

        // ✅ PUT: api/user/{id}/password
        [HttpPut("{id}/password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePasswordAsync(id, request.OldPassword, request.NewPassword);
            if (!result) return BadRequest(new { Message = "Password change failed" });

            return Ok(new { Message = "Password updated successfully" });
        }

        // ✅ DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (!result) return NotFound(new { Message = "User not found or already deleted" });

            return Ok(new { Message = "User deleted successfully" });
        }
    }

    // 📌 Extra payload models
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
