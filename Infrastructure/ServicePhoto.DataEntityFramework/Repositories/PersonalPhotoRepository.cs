using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PersonalPhotoRepository : EFRepository<PersonalPhoto>, IPersonalPhotoRepository
    {
        public PersonalPhotoRepository(AppDbContext appDbContext) : base(appDbContext) {}

        public async Task<PersonalPhoto> GetMainPhotoAsync(CancellationToken cancellationToken)
        {
            return await Entities.SingleAsync(it => it.IsMainPersonalPhoto, cancellationToken);
        }

        public async Task<PersonalPhoto?> FindPersonalPhotoAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == id, cancellationToken);
        }

        public async IAsyncEnumerable<PersonalPhoto> BySearch(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Entities.Where(c => c.AccountId == accountId).AsQueryable();
            await foreach (var photo in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
                yield return photo;
        }

    }
}
