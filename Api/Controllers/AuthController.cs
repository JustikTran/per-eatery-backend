using Application.IRepository;
using Domain.DTO;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("odata/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAccountRepository repository;
        private readonly IConfiguration configuration;
        public AuthController(IAccountRepository repository, IConfiguration configuration)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
            this.configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] DTOUserAuth request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await repository.SignIn(request);
            if (result.StatusCode == 200)
            {
                var tokenGenerator = new TokenGenerator(configuration);
                var token = tokenGenerator.GenerateToken(result.Data!);
                return Ok(new DTOResponse<object>
                {
                    StatusCode = 200,
                    Message = "Sign in successfully",
                    Data = new
                    {
                        User = result.Data,
                        AccessToken = token
                    }
                });
            }
            if (result.StatusCode != 401)
                return BadRequest(result);
            return Unauthorized(result);
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] DTOUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await repository.SignUp(request);
            if (result.StatusCode == 201)
            {
                return Created("sign up", result);
            }
            return BadRequest(result);
        }

        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] DTOUserAuth request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await repository.ForgotPassword(request);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
