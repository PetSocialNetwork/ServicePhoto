namespace ServicePhoto.WebApi.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadPhotoAsync(IFormFile file, CancellationToken cancellationToken);
        public void DeleteFiles(List<string> paths);
        void DeleteFile(string path);
    }
}
