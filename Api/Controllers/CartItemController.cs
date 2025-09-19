using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("odata/cart-item")]
    [ApiController]
    [Authorize]
    public class CartItemController : ODataController
    {
        private readonly ICartItemRepositoy repository;
        public CartItemController(ICartItemRepositoy cartItemRepository)
        {
            this.repository = cartItemRepository ?? throw new ArgumentException(nameof(cartItemRepository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOCartItemResponse>> GetAllCartItem()
        {
            var cartItems = repository.GetAllCartItem();
            return Ok(cartItems.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("cart-id={id}")]
        public async Task<IActionResult> GetByCartId([FromRoute] string id)
        {
            var response = await repository.GetByCartID(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCartItem([FromBody] DTOCartItemRequestCreate requestCreate)
        {
            if (requestCreate.CartId.IsNullOrEmpty())
                return BadRequest(new DTOResponse<DTOCartItemResponse>
                {
                    Message = "Cart ID is required.",
                    StatusCode = 400,
                    Data = null
                });
            var response = await repository.CreateCartItem(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] string id, [FromBody] DTOCartItemRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOCartItemResponse>
                {
                    Message = "ID does not match.",
                    StatusCode = 400,
                    Data = null
                });
            }
            var response = await repository.UpdateCartItem(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] string id)
        {
            var response = await repository.DeleteCartItem(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
