using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPersonalPhotoRepository : IRepositoryEF<PersonalPhoto>
    {
        Task<PersonalPhoto?> FindMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken);
        Task<PersonalPhoto?> FindPersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken);
        Task<List<PersonalPhoto>> BySearch(Guid profileId, PaginationOptions options, CancellationToken cancellationToken);
        Task<List<PersonalPhoto>> GetPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken);
        Task<List<PersonalPhoto>> FindMainPersonalPhotosByIdsAsync(Guid[] profileIds, CancellationToken cancellationToken);
    }
}
