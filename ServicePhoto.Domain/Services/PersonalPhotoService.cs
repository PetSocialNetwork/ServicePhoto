using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Exceptions;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.Domain.Services
{
    public class PersonalPhotoService
    {
        private readonly IPersonalPhotoRepository _personalPhotoRepository;
        public PersonalPhotoService(IPersonalPhotoRepository personalPhotoService)
        {
            _personalPhotoRepository = personalPhotoService
                ?? throw new ArgumentException(nameof(personalPhotoService));
        }

        public async Task<PersonalPhoto> AddAndSetPersonalPhotoAsync(PersonalPhoto photo, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(photo);

            var existedPhoto = await GetMainPersonalPhotoAsync(cancellationToken);
            existedPhoto.IsMainPersonalPhoto = false;
            await _personalPhotoRepository.Update(existedPhoto, cancellationToken);

            photo.IsMainPersonalPhoto = true;
            await _personalPhotoRepository.Add(photo, cancellationToken);
            return photo;
        }
        public async Task<PersonalPhoto> AddPersonalPhotoAsync(PersonalPhoto photo, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(photo);

            await _personalPhotoRepository.Add(photo, cancellationToken);
            return photo;
        }

        public async IAsyncEnumerable<PersonalPhoto>? BySearchPersonalPhotosAsync(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _personalPhotoRepository.BySearch(accountId, cancellationToken))
                yield return photo;
        }

        public async Task<PersonalPhoto> GetPersonalPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                return await _personalPhotoRepository.GetById(id, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
        }

        public async Task<PersonalPhoto> GetMainPersonalPhotoAsync(CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoRepository.GetMainPhotoAsync(cancellationToken);
            if (photo is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }

            return photo;
        }

        public async Task DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = await _personalPhotoRepository.FindPersonalPhotoAsync(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }

            await _personalPhotoRepository.Delete(existedPhoto, cancellationToken);
        }

        public async Task<PersonalPhoto> SetMainPersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            var photo = await GetMainPersonalPhotoAsync(cancellationToken);
            photo.IsMainPersonalPhoto = false;
            await _personalPhotoRepository.Update(photo, cancellationToken);
            var existedPhoto = await _personalPhotoRepository.GetById(photoId, cancellationToken);

            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            existedPhoto.IsMainPersonalPhoto = true;
            await _personalPhotoRepository.Update(existedPhoto, cancellationToken);
            return existedPhoto;
        }
    }
}
