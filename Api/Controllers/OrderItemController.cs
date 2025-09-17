using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("odata/order-item")]
    [ApiController]
    [Authorize]
    public class OrderItemController : ODataController
    {
        private readonly IOrderItemRepository repository;
        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            this.repository = orderItemRepository ?? throw new ArgumentException(nameof(orderItemRepository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOOrderItemResponse>> GetAllOrderItem()
        {
            var orderItems = repository.GetAllOrderItems();
            return Ok(orderItems.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("order-id={id}")]
        public async Task<IActionResult> GetByOrderId([FromRoute] string id)
        {
            var response = await repository.GetByOrderID(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderItem([FromBody] DTOOrderItemRequestCreate requestCreate)
        {
            if (requestCreate.OrderId.IsNullOrEmpty())
                return BadRequest(new DTOResponse<DTOOrderItemResponse>
                {
                    Message = "Order ID is required.",
                    StatusCode = 400,
                    Data = null
                });
            var response = await repository.CreateOrderItem(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateOrderItem([FromRoute] string id, [FromBody] DTOOrderItemRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOOrderItemResponse>
                {
                    Message = "ID does not match.",
                    StatusCode = 400,
                    Data = null
                });
            }
            var response = await repository.UpdateOrderItem(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

    }
}
