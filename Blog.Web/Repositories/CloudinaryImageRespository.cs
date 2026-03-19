using Blog.Web.Repositories.IRepository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Blog.Web.Repositories
{
    public class CloudinaryImageRespository : ICloudinaryImageRepository
    {
        private readonly IConfiguration _configuration = null;
        private readonly Account _account = null;

        public CloudinaryImageRespository(IConfiguration configuration)
        {
            _configuration = configuration;
            var section = _configuration.GetSection("Cloudinary");
            _account = new Account(
                section["CloudName"],
                section["ApiKey"],
                section["ApiSecret"]
                );

        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(_account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }
}
