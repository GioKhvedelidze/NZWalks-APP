using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageRepository _imageRepo;

    public ImageController(IImageRepository imageRepo)
    {
        _imageRepo = imageRepo;
    }

    [HttpPost]
    [Route("Upload")]
    public async Task<IActionResult> Upload(ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);

        if (ModelState.IsValid)
        {
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName
            };

            // Repository Method to upload image
            await _imageRepo.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        var allowedExtension = new string[] { ".jpg", ".jpeg", ".png"};

        if (!allowedExtension.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("file", "Unsupported file extension");
        }

        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("file", "file size is more than 10MB");
        }
    }
}