using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

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

        public async Task<IEnumerable<PetPhoto>> GetPetPhotosAsync(Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.Where(it => it.ProfileId == profileId && it.PetId == petId).ToListAsync(cancellationToken);
        }

        public async IAsyncEnumerable<PetPhoto> BySearch(Guid petId, Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Entities.Where(c => c.ProfileId == profileId && c.PetId ==petId && c.IsMainPetPhoto == false).AsQueryable();
            await foreach (var photo in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
                yield return photo;
        }
    }
}
