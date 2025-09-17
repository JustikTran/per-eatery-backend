using Api.VNPayService;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("odata/vnpay")]
    [ApiController]
    [Authorize]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService service;
        private readonly IConfiguration config;
        public VNPayController(IVNPayService vNPayHelper, IConfiguration config)
        {
            service = vNPayHelper;
            this.config = config;
        }

        [HttpGet("vnpay-return")]
        public ActionResult VNPayReturn()
        {
            var response = service.PaymentExecute(HttpContext.Request.Query);
            return response.VnPayResponseCode == "00" ? Redirect(config["Redirect:UrlSuccess"]!) : BadRequest(config["Redirect:UrlFail"]!);
        }


        [HttpPost("create-purchase")]
        public IActionResult CreateVNPay([FromBody] DTOVNPay request)
        {
            var paymentUrl = service.CreatePaymentUrl(request, HttpContext);
            return Ok(new { url = paymentUrl });
        }
    }
}
