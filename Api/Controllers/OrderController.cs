using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ODataController
    {
        private readonly IOrderRepository repository;
        public OrderController(IOrderRepository repository)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOOrderResponse>> GetAllOrder()
        {
            var orders = repository.GetAllOrder();
            return Ok(orders.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] DTOOrderRequestCreate requestCreate)
        {
            var response = await repository.CreateOrder(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string id, [FromBody] DTOOrderRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOOrderResponse>
                {
                    Message = "ID does not match.",
                    StatusCode = 400,
                    Data = null
                });
            }
            var response = await repository.UpdateOrder(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string id)
        {
            var response = await repository.DeleteOrder(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
