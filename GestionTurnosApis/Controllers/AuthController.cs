using GestionTurnosApis.Models;
using GestionTurnosApis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTurnosApis.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var token = _authService.Authenticate(user.Username, user.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }

        [HttpGet("GetUserByName/{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var userDetail = await _authService.GetUserByName(username);

            if (userDetail == null)
            {
                return NotFound($"No se encontró el usuario {username}.");
            }

            return Ok(userDetail);
        }
    }
}
