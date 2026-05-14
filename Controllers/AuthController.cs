using Microsoft.AspNetCore.Mvc;
using SOAP.Models;
using SOAP.Services;

namespace SOAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var token = await _authService.RegisterAsync(user);
            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _authService.LoginAsync(email, password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}
