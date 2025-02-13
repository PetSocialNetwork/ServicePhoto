using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PetPhotoRepository : EFRepository<PetPhoto>, IPetPhotoRepository
    {
        public PetPhotoRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<PetPhoto?> FindMainPhotoAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.IsMainPetPhoto && it.AccountId == accountId && it.PetId == petId, cancellationToken);
        }

        public async Task<PetPhoto?> FindPetPhotoAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<PetPhoto>> GetPetPhotosByAccountIdAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            return await Entities.Where(it => it.AccountId == accountId && it.PetId == petId).ToListAsync(cancellationToken);
        }

        public async IAsyncEnumerable<PetPhoto> BySearch(Guid petId, Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Entities.Where(c => c.AccountId == accountId && c.PetId ==petId && c.IsMainPetPhoto == false).AsQueryable();
            await foreach (var photo in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
                yield return photo;
        }
    }
}
