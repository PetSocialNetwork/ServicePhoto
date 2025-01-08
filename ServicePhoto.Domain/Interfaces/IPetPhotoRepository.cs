using ServicePhoto.Domain.Entities;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPetPhotoRepository : IRepositoryEF<PetPhoto>
    {
        Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken);
        Task<PetPhoto> GetMainPhotoAsync(CancellationToken cancellationToken);
        IAsyncEnumerable<PetPhoto> BySearch(Guid photoId, CancellationToken cancellationToken);
    }
}
