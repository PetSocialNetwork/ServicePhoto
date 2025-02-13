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

            var existedPhoto = await FindMainPetPhotoAsync(photo.PetId, photo.AccountId, cancellationToken);
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

        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = await _photoRepository.FindPetPhotoAsync(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }

            await _photoRepository.Delete(existedPhoto, cancellationToken);
        }

        public async Task DeleteAllPetPhotosAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            var photosToDelete = await _photoRepository.GetPetPhotosByAccountIdAsync(petId, accountId, cancellationToken);

            if (photosToDelete.Any())
            {
                foreach (var photo in photosToDelete)
                {
                    Uri uri = new Uri(photo.FilePath);
                    string relativePath = uri.AbsolutePath.TrimStart('/');
                    string filePath = "";

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                await _photoRepository.DeleteRange(photosToDelete, cancellationToken);
            }
        }

        public async Task<PetPhoto> SetMainPetPhotoAsync(Guid petId, Guid accountId, Guid photoId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            var photo = await FindMainPetPhotoAsync(petId, accountId, cancellationToken);
            if (photo is not null)
            {
                await _photoRepository.Delete(photo, cancellationToken);
            }

            var existedPhoto = await _photoRepository.GetById(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            existedPhoto.IsMainPetPhoto = true;
            await _photoRepository.Update(existedPhoto, cancellationToken);
            return existedPhoto;
        }

        public async IAsyncEnumerable<PetPhoto>? BySearchPetPhotosAsync(Guid petId, Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _photoRepository.BySearch(petId,accountId, cancellationToken))
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

        public async Task<PetPhoto?> FindMainPetPhotoAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.FindMainPhotoAsync(petId, accountId, cancellationToken);
            return photo;
        }
    }
}
