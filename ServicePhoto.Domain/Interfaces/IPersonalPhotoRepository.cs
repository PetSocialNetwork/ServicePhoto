using ServicePhoto.Domain.Entities;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPersonalPhotoRepository : IRepositoryEF<PersonalPhoto>
    {
        Task<PersonalPhoto?> FindPersonalPhotoAsync(Guid id, CancellationToken cancellationToken);
        Task<PersonalPhoto> GetMainPhotoAsync(CancellationToken cancellationToken);
        IAsyncEnumerable<PersonalPhoto> BySearch(Guid accountId, CancellationToken cancellationToken);
    }
}
