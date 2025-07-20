using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Exceptions;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.Domain.Services
{
    public class PetPhotoService
    {
        private readonly IPetPhotoRepository _photoRepository;
        private readonly IFileService _fileService;

        public PetPhotoService(IPetPhotoRepository photoRepository, IFileService fileService)
        {
            _photoRepository = photoRepository
                ?? throw new ArgumentException(nameof(photoRepository));
            _fileService = fileService 
                ?? throw new ArgumentNullException(nameof(fileService));
        }
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = await _photoRepository.FindPetPhotoAsync(photoId, cancellationToken)
                ?? throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            _fileService.DeleteFile(existedPhoto.FilePath);
            await _photoRepository.Delete(existedPhoto, cancellationToken);
        }
        public async Task DeleteAllPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            var photosToDelete = await _photoRepository.GetPetPhotosAsync
                (petId, profileId, cancellationToken);
            if (photosToDelete.Any())
            {
                List<string> pathsToDelete = photosToDelete.Select(p => p.FilePath).ToList();
                _fileService.DeleteFiles(pathsToDelete);
                await _photoRepository.DeleteRange(photosToDelete, cancellationToken);
            }
        }
        public async Task<PetPhoto> GetPetPhotoByIdAsync(Guid photoId, CancellationToken cancellationToken)
        {
            try
            {
                return await _photoRepository.GetById(photoId, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
        }
        public async Task<PetPhoto?> FindMainPetPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            return await _photoRepository.FindMainPhotoAsync(petId, profileId, cancellationToken);
        }

        public async Task<PetPhoto> AddAndSetPetPhotoAsync(PetPhoto photo, CancellationToken cancellationToken)
        {
            await DeleteMainPhotoAsync(photo.PetId, photo.ProfileId, cancellationToken);

            photo.FilePath = await _fileService.UploadPhotoAsync
                (photo.FileBytes, photo.OriginalFileName, cancellationToken);
            photo.IsMainPetPhoto = true;

            await _photoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async Task<PetPhoto> AddPetPhotoAsync(PetPhoto photo, CancellationToken cancellationToken)
        {
            photo.FilePath = await _fileService.UploadPhotoAsync
             (photo.FileBytes, photo.OriginalFileName, cancellationToken);
            photo.IsMainPetPhoto = false;

            await _photoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async Task<PetPhoto> SetMainPetPhotoAsync(Guid petId, Guid profileId, Guid photoId, CancellationToken cancellationToken)
        {
            await DeleteMainPhotoAsync(petId, profileId, cancellationToken);

            var existedPhoto = await _photoRepository.GetById(photoId, cancellationToken)
                ?? throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");

            existedPhoto.IsMainPetPhoto = true;
            await _photoRepository.Update(existedPhoto, cancellationToken);
            return existedPhoto;
        }

        public async Task<List<PetPhoto>> BySearchAsync
            (Guid petId, Guid profileId, PaginationOptions options, CancellationToken cancellationToken)
        {
            return await _photoRepository.BySearch(petId, profileId, options, cancellationToken);
        }

        private async Task DeleteMainPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            var photo = await FindMainPetPhotoAsync(petId, profileId, cancellationToken);
            if (photo is not null)
            {
                _fileService.DeleteFile(photo.FilePath);
                await _photoRepository.Delete(photo, cancellationToken);
            }
        }
    }
}
