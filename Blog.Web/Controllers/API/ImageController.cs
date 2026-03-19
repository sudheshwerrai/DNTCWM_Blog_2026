using Blog.Web.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Blog.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ICloudinaryImageRepository _imageRepository = null;
        public ImageController(ICloudinaryImageRepository imageRepository)
        {
            _imageRepository=imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageUrl=await _imageRepository.UploadAsync(file);
            return imageUrl == null ?
                Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError)
                : new JsonResult(new { link = imageUrl });
        }
    }
}
