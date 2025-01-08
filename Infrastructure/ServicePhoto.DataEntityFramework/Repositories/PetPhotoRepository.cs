using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PetPhotoRepository : EFRepository<PetPhoto>, IPetPhotoRepository
    {
        public PetPhotoRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<PetPhoto> GetMainPhotoAsync(CancellationToken cancellationToken)
        {
            return await Entities.SingleAsync(it => it.IsMainPetPhoto, cancellationToken);
        }

        public async Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == id, cancellationToken);
        }

        public async IAsyncEnumerable<PetPhoto> BySearch(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Entities.Where(c => c.AccountId == accountId).AsQueryable();
            await foreach (var photo in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
                yield return photo;
        }
    }
}
