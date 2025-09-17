using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/cart")]
    [ApiController]
    public class CartController : ODataController
    {
        private readonly ICartRepository repository;
        public CartController(ICartRepository cartRepository)
        {
            this.repository = cartRepository ?? throw new ArgumentException(nameof(cartRepository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOCartResponse>> GetAllCarts()
        {
            var carts = repository.GetAllCart();
            return Ok(carts.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] DTOCartRequestCreate requestCreate)
        {
            var response = await repository.CreateCart(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateCart([FromRoute] string id, [FromBody] DTOCartRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOCartResponse>
                {
                    Message = "ID does not match.",
                    StatusCode = 400,
                    Data = null
                });
            }
            var response = await repository.UpdateCart(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteCart([FromRoute] string id)
        {
            var response = await repository.DeleteCart(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
