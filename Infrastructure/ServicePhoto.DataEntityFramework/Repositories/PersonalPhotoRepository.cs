using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using System.Runtime.CompilerServices;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PersonalPhotoRepository : EFRepository<PersonalPhoto>, IPersonalPhotoRepository
    {
        public PersonalPhotoRepository(AppDbContext appDbContext) : base(appDbContext) {}
        public async Task<PersonalPhoto?> FindMainPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == profileId && it.IsMainPersonalPhoto == true, cancellationToken);
        }

        public async Task<PersonalPhoto?> FindPersonalPhotoAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == profileId, cancellationToken);
        }

        public async Task<IEnumerable<PersonalPhoto>> GetPersonalPhotosAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.Where(it => it.Id == profileId).ToListAsync(cancellationToken);
        }

        public async IAsyncEnumerable<PersonalPhoto> BySearch(Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var query = Entities.Where(c => c.Id == profileId && c.IsMainPersonalPhoto == false).AsQueryable();
            await foreach (var photo in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
                yield return photo;
        }

    }
}
