using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.Domain.Interfaces
{
    public interface IPetPhotoRepository : IRepositoryEF<PetPhoto>
    {
        Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken);
        Task<PetPhoto?> FindMainPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken);
        Task<List<PetPhoto>> BySearch(Guid petId, Guid profileId, PaginationOptions options, CancellationToken cancellationToken);
        Task<List<PetPhoto>> GetPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken);
    }
}
