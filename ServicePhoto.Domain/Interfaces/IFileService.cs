namespace ServicePhoto.Domain.Interfaces
{
    public interface IFileService
    {
        Task <string> UploadPhotoAsync(byte[] file, string originalFileName, CancellationToken cancellationToken);
        public void DeleteFiles(List<string> paths);
        void DeleteFile(string path);
    }
}
