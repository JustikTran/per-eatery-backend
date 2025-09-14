using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/dish")]
    [ApiController]
    public class DishController : ODataController
    {
        private readonly IDishRepository repository;

        public DishController(IDishRepository repository)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet]
        [AllowAnonymous]
        [EnableQuery]
        public ActionResult<IEnumerable<DTODishResponse>> GetAllDishes()
        {
            var listDishes = repository.GetAllDish();
            return Ok(listDishes.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create-dish")]
        public async Task<IActionResult> CreateDish([FromBody] DTODishRequestCreate requestCreate)
        {
            var response = await repository.CreateDish(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] string id, [FromBody] DTODishRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<object>
                {
                    StatusCode = 400,
                    Message = "Id does not match."
                });
            }
            var response = await repository.UpdateDish(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteDish([FromRoute] string id)
        {
            var response = await repository.DeleteDish(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
