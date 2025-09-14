using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("odata/upload-image")]
    [ApiController]
    [AllowAnonymous]
    public class UploadImageController : ControllerBase
    {
        private readonly Cloudinary cloudinary;
        public UploadImageController(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            await using var stream = file.OpenReadStream();
            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "eatery_images" // Optional: specify a folder in Cloudinary
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new 
                {
                    StatusCode = 201,
                    ImageUrl = uploadResult.SecureUrl.ToString()
                });
            }
            else
            {
                return StatusCode((int)uploadResult.StatusCode, new
                {
                    StatusCode = (int)uploadResult.StatusCode,
                    Message = "Image upload failed."
                });
            }
        }
    }
}
