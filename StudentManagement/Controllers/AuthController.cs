using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Authentication")]
    public class AuthController(IAuthService service) : ControllerBase
    {
        private readonly IAuthService _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReq request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest(new { message = "Name is required." });

                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Email is required." });

                if (string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { message = "Password is required." });

                var data = await _service.Register(request);

                if (data == null)
                    return BadRequest(new { message = "Email already exists." });

                return Ok(new { message = "Staff registered successfully.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while registering the staff." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                    return BadRequest(new { message = "Email is required." });

                if (string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { message = "Password is required." });

                var data = await _service.Login(request);

                if (data == null)
                    return BadRequest(new { message = "Invalid email or password." });

                return Ok(new { message = "Login successful.", data });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while logging in." });
            }
        }
    }
}