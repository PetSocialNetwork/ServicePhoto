using ServicePhoto.Domain.Entities;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPetPhotoRepository : IRepositoryEF<PetPhoto>
    {
        Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken);
        Task<PetPhoto?> FindMainPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken);
        IAsyncEnumerable<PetPhoto> BySearch(Guid petId, Guid profileId, CancellationToken cancellationToken);
        Task<IEnumerable<PetPhoto>> GetPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken);
    }
}
