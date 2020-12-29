using Microsoft.AspNetCore.Mvc;
using TaskList.Core.Interfaces.Services;
using TaskList.Models;

namespace TaskList.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAccountService accountService;

        public AuthController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var userId = accountService.Login(model.UserName, model.Password);

            if (userId == -1)
            {
                return this.Unauthorized();
            }

            var token = accountService.GenerateToken(model.UserName, userId);

            this.HttpContext.Response.Headers.Add("Authorization", token);

            return this.Ok(new UserViewModel { Id = userId, UserName = model.UserName });
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            accountService.Logout();
            return this.Ok();
        }
    }
}
