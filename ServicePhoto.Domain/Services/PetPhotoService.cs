using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Exceptions;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.Domain.Services
{
    public class PetPhotoService
    {
        private readonly IPetPhotoRepository _photoRepository;

        public PetPhotoService(IPetPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository
                ?? throw new ArgumentException(nameof(photoRepository));
        }

        public async Task<PetPhoto> AddAndSetPetPhotoAsync(PetPhoto photo, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(photo);

            var existedPhoto = await FindMainPetPhotoAsync(photo.PetId, photo.ProfileId, cancellationToken);
            if (existedPhoto != null)
            {
                await _photoRepository.Delete(existedPhoto, cancellationToken);
            }

            photo.IsMainPetPhoto = true;
            await _photoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async Task<PetPhoto> AddPetPhotoAsync(PetPhoto photo, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(photo);

            await _photoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async Task<string> DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = await _photoRepository.FindPetPhotoAsync(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            var filePath = existedPhoto.FilePath;
            await _photoRepository.Delete(existedPhoto, cancellationToken);
            return filePath;
        }

        public async Task<List<string>> DeleteAllPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            var photosToDelete = await _photoRepository.GetPetPhotosAsync(petId, profileId, cancellationToken);
            List<string> pathsToDelete = photosToDelete.Select(p => p.FilePath).ToList();
            await _photoRepository.DeleteRange(photosToDelete, cancellationToken);
            return pathsToDelete;
        }


        public async IAsyncEnumerable<PetPhoto>? BySearchAsync(Guid petId, Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _photoRepository.BySearch(petId, profileId, cancellationToken))
                yield return photo;
        }

        public async Task<PetPhoto> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                return await _photoRepository.GetById(id, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
        }

        public async Task<PetPhoto?> FindMainPetPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.FindMainPhotoAsync(petId, profileId, cancellationToken);
            return photo;
        }

        public async Task<(PetPhoto, string)> SetMainPetPhotoAsync(Guid petId, Guid profileId, Guid photoId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            string oldFilePath = string.Empty;
            var photo = await FindMainPetPhotoAsync(petId, profileId, cancellationToken);
            if (photo is not null)
            {
                oldFilePath = photo.FilePath;
                await _photoRepository.Delete(photo, cancellationToken);
            }

            var existedPhoto = await _photoRepository.GetById(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            existedPhoto.IsMainPetPhoto = true;
            await _photoRepository.Update(existedPhoto, cancellationToken);
            return (existedPhoto, oldFilePath);
        }
    }
}
