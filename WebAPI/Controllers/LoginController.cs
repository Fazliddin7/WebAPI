using Application.DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebApplication3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        public LoginController(IUserService userService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger; ;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");

                var result = await _userService.Authenticate(loginDTO);

                _logger.LogInformation($"Login - user: {loginDTO.UserName} - logginig");
                return Ok(new
                {
                    result
                });
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register(RegisterUserRequest loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
                    return BadRequest("Username and/or Password not specified");

                await _userService.Register(loginDTO);

                _logger.LogInformation($"Register - user: {loginDTO.UserName} - Register");
                return Ok();
            }
            catch
            {
                return BadRequest
                ("An error occurred in generating the token");
            }
        }

        [HttpGet, Route("Roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _userService.GetRoles();
            return Ok(roles);
        }
    }
}
