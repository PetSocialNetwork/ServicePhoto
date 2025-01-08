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

            var existedPhoto = await GetMainPetPhotoAsync(cancellationToken);
            existedPhoto.IsMainPetPhoto = false;
            await _photoRepository.Update(existedPhoto, cancellationToken);

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

        public async IAsyncEnumerable<PetPhoto>? BySearchPetPhotosAsync(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _photoRepository.BySearch(accountId, cancellationToken))
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

        public async Task<PetPhoto> GetMainPetPhotoAsync(CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetMainPhotoAsync(cancellationToken);
            if (photo is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }

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

        public async Task<PetPhoto> SetMainPetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            var photo = await GetMainPetPhotoAsync(cancellationToken);
            photo.IsMainPetPhoto = false;
            await _photoRepository.Update(photo, cancellationToken);
            var existedPhoto = await _photoRepository.GetById(photoId, cancellationToken);

            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            existedPhoto.IsMainPetPhoto = true;
            await _photoRepository.Update(existedPhoto, cancellationToken);
            return existedPhoto;
        }
    }
}
