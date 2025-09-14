using Application.IRepository;
using Application.IService;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("odata/verify-code")]
    [ApiController]
    public class VerifyCodeController : ControllerBase
    {
        private IVerifyCodeRepository repository;
        private IElasticEmailService service;

        public VerifyCodeController(IVerifyCodeRepository repository, IElasticEmailService service)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
            this.service = service ?? throw new ArgumentException(nameof(service));
        }

        [HttpPost("send-mail")]
        public async Task<IActionResult> SendEmail([FromBody] DTOCodeRequest codeRequest)
        {
            var code = await repository.SaveCode(codeRequest);
            if (code == null)
            {
                return BadRequest(new DTOResponse<object>
                {
                    StatusCode = 400,
                    Message = "Create OTP code failure."
                });
            }

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailOTPVerify.html");

            await service.SendEmailAsync(code, templatePath);
            return Ok(new DTOResponse<object>
            {
                StatusCode = 200,
                Message = "Verification email sent successfully."
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] DTOVerifyCodeRequest verifyRequest)
        {
            var response = await repository.VerifyCode(verifyRequest);
            return StatusCode(response.StatusCode, response);
        }

    }
}
