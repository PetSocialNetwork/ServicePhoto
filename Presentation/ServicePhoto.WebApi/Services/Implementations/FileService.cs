using ServicePhoto.WebApi.Services.Interfaces;

namespace ServicePhoto.WebApi.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FileService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentException(nameof(webHostEnvironment));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }
        public async Task<string> UploadPhotoAsync(IFormFile file, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(file));

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            var photoBaseUrl = _configuration["PhotoBaseUrl"];

            if (string.IsNullOrEmpty(photoBaseUrl))
            {
                throw new InvalidOperationException("Установите базовый адрес в appsettings.json.");
            }

            var fullUrl = $"{photoBaseUrl}{fileName}";
            return fullUrl;
        }


        public void DeleteFiles(List<string> paths)
        {
            if (paths.Count != 0)
            {
                foreach (var path in paths)
                {
                    DeleteFile(path);
                }
            }
        }

        public void DeleteFile(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            var webRootPath = _webHostEnvironment.WebRootPath;

            Uri uri = new(path);
            string fileName = Path.GetFileName(uri.AbsolutePath);
            string filePath = Path.Combine(webRootPath, "images", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
