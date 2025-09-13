using Application.IRepository;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers
{
    [Route("odata/user")]
    [ApiController]
    [Authorize]
    public class UserController : ODataController
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DTOUserResponse>> GetAll()
        {
            var users = _userRepository.GetAllUser();
            return Ok(users);
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _userRepository.GetUserById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("username{id}")]
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            var result = await _userRepository.ExistUsername(username);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("email={email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var result = await _userRepository.ExistEmail(email);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] DTOUserUpdate userUpdate)
        {
            if (id != userUpdate.Id)
            {
                return BadRequest(new DTOResponse<DTOUserResponse>
                {
                    Message = "Value of id does not match.",
                    StatusCode = 400,
                    Data = null
                });
            }
            var result = await _userRepository.UpdateUser(userUpdate);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] DTOUserChangePassword changePassword)
        {
            var result = await _userRepository.ChangePassword(changePassword);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var result = await _userRepository.DeleteUser(id);
            return StatusCode(result.StatusCode, result);
        }

    }
}
