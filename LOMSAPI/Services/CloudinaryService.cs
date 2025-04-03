using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace LOMSAPI.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult == null)
                    return "Upload thất bại! Kiểm tra API Key hoặc kết nối mạng.";

                if (uploadResult.Error != null)
                    return $"Lỗi Cloudinary: {uploadResult.Error.Message}";

                return uploadResult.SecureUrl?.ToString() ?? "Không lấy được URL!";
                
            }
        }
    }
}
