using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userApi.DbContext.Configuration;
using userApi.Models.Users;
using userApi.Services.Users;
using WebApi.Models.Users;

namespace userApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = userService.Authenticate(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpGet]
        [Authorize(Roles = ClaimConfiguration.AdminRoleName)]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ClaimConfiguration.EditorRoleName)]
        public IActionResult GetById(int id)
        {
            var user = userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ClaimConfiguration.EditorRoleName)]
        public IActionResult Update(int id, UpdateRequest model)
        {
            userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ClaimConfiguration.AdminRoleName)]
        public IActionResult Delete(int id)
        {
            userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
