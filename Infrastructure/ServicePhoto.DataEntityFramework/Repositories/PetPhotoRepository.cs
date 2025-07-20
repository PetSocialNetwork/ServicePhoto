using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PetPhotoRepository : EFRepository<PetPhoto>, IPetPhotoRepository
    {
        public PetPhotoRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<PetPhoto?> FindMainPhotoAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.IsMainPetPhoto && it.ProfileId == profileId && it.PetId == petId, cancellationToken);
        }

        public async Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == id, cancellationToken);
        }

        public async Task<List<PetPhoto>> GetPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.Where(it => it.ProfileId == profileId && it.PetId == petId).ToListAsync(cancellationToken);
        }

        public async Task<List<PetPhoto>> BySearch(Guid petId, Guid profileId, PaginationOptions options, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(c => c.ProfileId == profileId && c.PetId == petId && c.IsMainPetPhoto == false)
                .Skip(options.Take * options.Offset)
                .Take(options.Take)
                .ToListAsync(cancellationToken);
        }
    }
}
