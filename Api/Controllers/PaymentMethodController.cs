using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/payment-method")]
    [ApiController]
    [Authorize]
    public class PaymentMethodController : ODataController
    {
        private readonly IPaymentMethodRepository repository;
        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            this.repository = paymentMethodRepository ?? throw new ArgumentException(nameof(paymentMethodRepository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOPaymentMethodResponse>> GetAllMethod()
        {
            var methods = repository.GetAllPayment();
            return Ok(methods.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<ActionResult<DTOResponse<DTOPaymentMethodResponse>>> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTOResponse<DTOPaymentMethodResponse>>> CreateMethod([FromBody] DTOPaymentMethodRequestCreate request)
        {
            var response = await repository.CreatePayment(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTOResponse<DTOPaymentMethodResponse>>> UpdateMethod([FromRoute] string id, [FromBody] DTOPaymentMethodRequestUpdate request)
        {
            if (id != request.Id)
            {
                return BadRequest(new DTOResponse<DTOPaymentMethodResponse>
                {
                    Message = "Id does not match.",
                    Data = null,
                    StatusCode = 400
                });
            }
            var response = await repository.UpdatePayment(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InActiveMethod([FromRoute] string id)
        {
            var response = await repository.InActivePayment(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
