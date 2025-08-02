using Microsoft.Extensions.Options;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.FileStorage.Configurations;

namespace ServicePhoto.FileStorage.Services
{
    public class FileService : IFileService
    {
        private readonly string _webRootPath;
        private readonly string _photoBaseUrl;
        private readonly string _photoDirectory;

        public FileService(IOptions<FileSettings> settings)
        {
            _webRootPath = settings.Value.WebRootPath
                ?? throw new ArgumentException(nameof(settings.Value.WebRootPath));
            _photoBaseUrl = settings.Value.PhotoBaseUrl
                ?? throw new ArgumentException(nameof(settings.Value.PhotoBaseUrl));
            _photoDirectory = settings.Value.PhotoDirectory
                ?? throw new ArgumentException(nameof(settings.Value.PhotoDirectory));
        }

        public async Task<string> UploadPhotoAsync
            (byte[] file, string originalFileName, CancellationToken cancellationToken)
        {
            string uploadsFolder = Path.Combine(_webRootPath, _photoDirectory);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string fileName = Guid.NewGuid() + Path.GetExtension(originalFileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            await File.WriteAllBytesAsync(filePath, file, cancellationToken);

            if (string.IsNullOrEmpty(_photoBaseUrl))
            {
                throw new InvalidOperationException("Установите базовый адрес в appsettings.json.");
            }

            var fullUrl = $"{_photoBaseUrl}{fileName}";
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
            Uri uri = new(path);
            string fileName = Path.GetFileName(uri.AbsolutePath);
            string filePath = Path.Combine(_webRootPath, _photoDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
