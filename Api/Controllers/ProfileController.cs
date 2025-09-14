using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : ODataController
    {
        private readonly IProfileRepository repository;

        public ProfileController(IProfileRepository repository)
        {
            this.repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet]
        public ActionResult<IEnumerable<DTOProfileResponse>> GetAllProfile()
        {
            var listProfiles = repository.GetAllProfile();
            return Ok(listProfiles.AsQueryable());
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var response = await repository.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("create-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] DTOProfileRequest createRequest)
        {
            var response = await repository.CreateProfile(createRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] string id, [FromBody] DTOProfileRequest updateRequest)
        {
            var response = await repository.UpdateProfile(updateRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute] string id)
        {
            var response = await repository.DeleteProfile(id);
            return StatusCode(response.StatusCode, response);
        }

    }
}
