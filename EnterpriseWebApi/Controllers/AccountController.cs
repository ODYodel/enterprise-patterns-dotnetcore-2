using System.Security.Claims;
using System.Threading.Tasks;
using Enterprise.DTO;
using Enterprise.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDTO userRegistration)
        {
            if (ModelState.IsValid)
            {
                var isRegistered = await _authenticationService.RegisterUser(userRegistration);
                if (isRegistered)
                {
                    return Ok("You are logged in");
                }
                else
                {
                    return BadRequest("You are not in :(");
                }
            }
            return BadRequest("Don't know what happened. ELK Stack this or New Relic it.");
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("PasswordLogin")]
        public async Task<IActionResult> PasswordLogin([FromBody]UserPasswordLoginDTO userPasswordLoginDTO)
        {
            var isLoggedIn = await _authenticationService.PasswordLoginUser(userPasswordLoginDTO);
            if (isLoggedIn)
            {
                var user = User;
                return Ok($"I'm in!");
            }
            else
            {
                return BadRequest("I'm out :(");
            }
        }
        [HttpGet]
        [Route("Claims")]
        public IActionResult Claims()
        {
            var userclaims = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ;
            return Ok($"{userclaims}");
        }
    }
}
