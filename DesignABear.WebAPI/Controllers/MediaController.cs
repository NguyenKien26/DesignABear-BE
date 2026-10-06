using DesignABear.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/media")]
    [Produces("application/json")]
    public class MediaController : BaseController
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly IConfiguration _configuration;

        public MediaController(Supabase.Client supabaseClient, IConfiguration configuration)
        {
            _supabaseClient = supabaseClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Upload a media file of any format/size to Supabase storage
        /// </summary>
        /// <param name="file">The media file to upload</param>
        /// <param name="folder">Optional folder path (default: 'uploads')</param>
        /// <returns>Upload result containing the file's public URL</returns>
        [HttpPost("upload")]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        [SwaggerOperation(Summary = "Upload media file", Description = "Uploads any media file to Supabase storage without size or format restrictions")]
        [SwaggerResponse(200, "File uploaded successfully")]
        [SwaggerResponse(400, "Invalid file or request")]
        [SwaggerResponse(500, "Upload failed")]
        public async Task<IActionResult> UploadMedia(IFormFile file, [FromQuery] string folder = "uploads")
        {
            if (file == null || file.Length == 0)
            {
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "No file provided.")));
            }

            // No format limitation, no size limitation (via [DisableRequestSizeLimit])

            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var cleanFolder = folder?.Trim().Trim('/') ?? "";
            // Always ensure the path starts with 'media/' as requested
            var filePath = string.IsNullOrEmpty(cleanFolder) ? $"media/{fileName}" : $"media/{cleanFolder}/{fileName}";

            // InitializeAsync is not required for a ServiceRole Singleton client
            // Ensure the client is initialized to set up internal storage URLs correctly
            await _supabaseClient.InitializeAsync();
            var storage = _supabaseClient.Storage;
            var bucketName = _configuration["Supabase:Storage:BucketName"] ?? "media";

            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var result = await storage
                .From(bucketName)
                .Upload(fileBytes, filePath, new Supabase.Storage.FileOptions
                {
                    CacheControl = "3600",
                    Upsert = false
                });

            if (result == null)
            {
                return HandleResult(Result<object>.Failure(new Error("UploadFailed", "Failed to upload file to storage.")));
            }

            var publicUrl = storage
                .From(bucketName)
                .GetPublicUrl(filePath);

            var response = new 
            {
                FileName = fileName,
                FilePath = filePath,
                PublicUrl = publicUrl,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            return HandleResult(Result<object>.Success(response));
        }
    }
}
