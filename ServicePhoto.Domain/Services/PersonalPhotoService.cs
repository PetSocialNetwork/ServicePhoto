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

            var existedPhoto = await FindMainPersonalPhotoAsync(photo.ProfileId, cancellationToken);
            if (existedPhoto != null)
            {
                await _personalPhotoRepository.Delete(existedPhoto, cancellationToken);
            }

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

        public async Task<string> DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var existedPhoto = await _personalPhotoRepository.FindPersonalPhotoAsync(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }

            var path = existedPhoto.FilePath;
            await _personalPhotoRepository.Delete(existedPhoto, cancellationToken);
            return path;
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

        public async Task<PersonalPhoto?> FindMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await _personalPhotoRepository.FindMainPersonalPhotoAsync(profileId, cancellationToken);
        }

        public async Task<List<PersonalPhoto>> FindMainPersonalPhotosByIdsAsync(Guid[] profileIds, CancellationToken cancellationToken)
        {
            return await _personalPhotoRepository.FindMainPersonalPhotosByIdsAsync(profileIds, cancellationToken);
        }

        public async IAsyncEnumerable<PersonalPhoto>? BySearchPhotosAsync(Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _personalPhotoRepository.BySearch(profileId, cancellationToken))
                yield return photo;
        }

        public async Task<List<string>> DeleteAllPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken)
        {
            var photosToDelete = await _personalPhotoRepository.GetPersonalPhotosAsync(profileId, cancellationToken); ;
            List<string> pathsToDelete = photosToDelete.Select(p => p.FilePath).ToList();
            await _personalPhotoRepository.DeleteRange(photosToDelete, cancellationToken);
            return pathsToDelete;
        }

        public async Task<(PersonalPhoto, string)> SetMainPersonalPhotoAsync(Guid profileId, Guid photoId, CancellationToken cancellationToken)
        {
            //TODO: Транзакция
            string oldFilePath = string.Empty;
            var photo = await FindMainPersonalPhotoAsync(profileId, cancellationToken);
            if (photo is not null)
            {
                oldFilePath = photo.FilePath;
                await _personalPhotoRepository.Delete(photo, cancellationToken);
            }

            var existedPhoto = await _personalPhotoRepository.GetById(photoId, cancellationToken);
            if (existedPhoto is null)
            {
                throw new PhotoNotFoundException("Фотографии с таким идентификатором не существует.");
            }
            existedPhoto.IsMainPersonalPhoto = true;
            await _personalPhotoRepository.Update(existedPhoto, cancellationToken);
            return (existedPhoto, oldFilePath);
        }
    }
}
