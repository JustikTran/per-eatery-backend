using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ODataController
    {
        private readonly IAddressRepository repository;
        public AddressController(IAddressRepository repository)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOAddressReceiveResponse>> GetAllAddress()
        {
            var addresses = repository.GetAllAddress();
            return Ok(addresses.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAddress([FromBody] DTOAddressRequestCreate requestCreate)
        {
            var response = await repository.CreateAddress(requestCreate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] string id, [FromBody] DTOAddressRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOAddressReceiveResponse>
                {
                    Message = "Id in route does not match Id in body",
                    StatusCode = 400,
                    Data = null
                });
            }
            var response = await repository.UpdateAddress(requestUpdate);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute]string id)
        {
            var response = await repository.DeleteAddress(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
