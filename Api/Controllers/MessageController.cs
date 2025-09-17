using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/message")]
    [ApiController]
    [Authorize]
    public class MessageController : ODataController
    {
        private readonly IMessageRepository repository;
        public MessageController(IMessageRepository messageRepository)
        {
            this.repository = messageRepository ?? throw new ArgumentException(nameof(messageRepository));
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<DTOMessageResponse>> GetAllMessage()
        {
            var result = repository.GetAllMessage();
            return Ok(result.AsQueryable());
        }

        [HttpGet("user-id={id}")]
        public async Task<IActionResult> GetByUserId([FromRoute] string id)
        {
            var result = await repository.GetByUserId(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMessage([FromBody] DTOMessageRequestCreate requestCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await repository.CreateMessage(requestCreate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateMessage([FromRoute] string id, [FromBody] DTOMessageRequestUpdate requestUpdate)
        {
            if (id != requestUpdate.Id)
            {
                return BadRequest("Id in route and body do not match");
            }
            var result = await repository.UpdateMessage(requestUpdate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] string id)
        {
            var result = await repository.DeleteMessage(id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
