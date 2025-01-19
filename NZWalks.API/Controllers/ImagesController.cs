using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
           ValidateFileUpload(imageUploadRequestDto);

            if(ModelState.IsValid)
            {
                //convert DTO to Domain Model
                var imageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.File.FileName,
                    FileDiscription = imageUploadRequestDto.FileDiscription,
                };

                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);

                //User Repository to upload image 
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowedExtention = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtention.Contains(Path.GetExtension(imageUploadRequestDto.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported File Extension");
            }

            if (imageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is morethan 10MB, please upload a smaller size file");
            }
        }
    }
}
