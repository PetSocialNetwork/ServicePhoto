using ServicePhoto.Domain.Entities;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPersonalPhotoRepository : IRepositoryEF<PersonalPhoto>
    {
        Task<PersonalPhoto?> FindMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken);
        Task<PersonalPhoto?> FindPersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken);
        IAsyncEnumerable<PersonalPhoto> BySearch(Guid profileId, CancellationToken cancellationToken);
        Task<IEnumerable<PersonalPhoto>> GetPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken);
    }
}
