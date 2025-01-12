using ServicePhoto.Domain.Entities;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPetPhotoRepository : IRepositoryEF<PetPhoto>
    {
        Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken);
        Task<PetPhoto?> FindMainPhotoAsync(Guid petId, Guid accountId, CancellationToken cancellationToken);
        IAsyncEnumerable<PetPhoto> BySearch(Guid petId, Guid accountId, CancellationToken cancellationToken);
        Task<IEnumerable<PetPhoto>> GetPetPhotosByAccountIdAsync(Guid petId, Guid accountId, CancellationToken cancellationToken);
    }
}
