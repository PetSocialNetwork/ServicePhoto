using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Exceptions;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.Domain.Services
{
    public class PersonalPhotoService
    {
        private readonly IPersonalPhotoRepository _personalPhotoRepository;
        private readonly IFileService _fileService;
        public PersonalPhotoService(IPersonalPhotoRepository personalPhotoService,
            IFileService fileService)
        {
            _personalPhotoRepository = personalPhotoService
                ?? throw new ArgumentException(nameof(personalPhotoService));
            _fileService = fileService
                ?? throw new ArgumentException(nameof(fileService));
        }

        public async Task DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = 
                await _personalPhotoRepository.FindPersonalPhotoAsync(photoId, cancellationToken) 
                ?? throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            _fileService.DeleteFile(existedPhoto.FilePath);
            await _personalPhotoRepository.Delete(existedPhoto, cancellationToken);
        }

        public async Task<PersonalPhoto?> FindMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await _personalPhotoRepository.FindMainPersonalPhotoAsync(profileId, cancellationToken);
        }

        public async Task<List<PersonalPhoto>> FindMainPersonalPhotosByIdsAsync(Guid[] profileIds, CancellationToken cancellationToken)
        {
            return await _personalPhotoRepository.FindMainPersonalPhotosByIdsAsync(profileIds, cancellationToken);
        }

        public async Task<PersonalPhoto> SetMainPersonalPhotoAsync(Guid profileId, Guid photoId, CancellationToken cancellationToken)
        {
            var photo = await FindMainPersonalPhotoAsync(profileId, cancellationToken);

            if (photo is not null)
            {
                _fileService.DeleteFile(photo.FilePath);
                await _personalPhotoRepository.Delete(photo, cancellationToken);
            }

            var existedPhoto = await _personalPhotoRepository.GetById(photoId, cancellationToken)
                ?? throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");

            existedPhoto.IsMainPersonalPhoto = true;
            await _personalPhotoRepository.Update(existedPhoto, cancellationToken);
            return existedPhoto;
        }

        public async Task DeleteAllPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken)
        {
            var photosToDelete = await _personalPhotoRepository
                .GetPersonalPhotosAsync(profileId, cancellationToken);
            if (photosToDelete.Any())
            {
                List<string> pathsToDelete = photosToDelete.Select(p => p.FilePath).ToList();
                await _personalPhotoRepository.DeleteRange(photosToDelete, cancellationToken);
                _fileService.DeleteFiles(pathsToDelete);
            }
        }

        public async Task<PersonalPhoto> GetPersonalPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken)
        {
            try
            {
                return await _personalPhotoRepository.GetById(photoId, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
        }

        public async Task<List<PersonalPhoto>> BySearchPhotosAsync(Guid profileId, PaginationOptions options, CancellationToken cancellationToken)
        {
            return await _personalPhotoRepository.BySearch(profileId, options, cancellationToken);
        }

        public async Task<PersonalPhoto> AddAndSetPersonalPhotoAsync(
           PersonalPhoto photo, 
           CancellationToken cancellationToken)
        {      
            var existedPhoto = await FindMainPersonalPhotoAsync(photo.ProfileId, cancellationToken);
            if (existedPhoto != null)
            {
                _fileService.DeleteFile(existedPhoto.FilePath);
                await _personalPhotoRepository.Delete(existedPhoto, cancellationToken);
            }

            photo.FilePath = await _fileService.UploadPhotoAsync
              (photo.FileBytes, photo.OriginalFileName, cancellationToken);
            photo.IsMainPersonalPhoto = true;

            await _personalPhotoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async Task<PersonalPhoto> AddPersonalPhotoAsync(
           PersonalPhoto photo,
           CancellationToken cancellationToken)
        {
            photo.FilePath = await _fileService.UploadPhotoAsync
                (photo.FileBytes, photo.OriginalFileName, cancellationToken);
            photo.IsMainPersonalPhoto = false;

            await _personalPhotoRepository.Add(photo, cancellationToken);
            return photo;
        }
    }
}
