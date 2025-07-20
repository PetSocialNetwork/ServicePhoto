using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Interfaces;
using ServicePhoto.Domain.Shared;

namespace ServicePhoto.DataEntityFramework.Repositories
{
    public class PersonalPhotoRepository : EFRepository<PersonalPhoto>, IPersonalPhotoRepository
    {
        public PersonalPhotoRepository(AppDbContext appDbContext) : base(appDbContext) {}
        public async Task<PersonalPhoto?> FindMainPersonalPhotoAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.ProfileId == profileId && it.IsMainPersonalPhoto == true, cancellationToken);
        }

        public async Task<List<PersonalPhoto>> FindMainPersonalPhotosByIdsAsync
            (Guid[] profileIds, CancellationToken cancellationToken)
        {
            return await Entities
                .Where(it => profileIds.Contains(it.ProfileId) && it.IsMainPersonalPhoto == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<PersonalPhoto?> FindPersonalPhotoAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
            return await Entities.SingleOrDefaultAsync(it => it.Id == photoId, cancellationToken);
        }

        public async Task<List<PersonalPhoto>> GetPersonalPhotosAsync
            (Guid profileId, CancellationToken cancellationToken)
        {
            return await Entities.Where(it => it.ProfileId == profileId).ToListAsync(cancellationToken);
        }

        public async Task<List<PersonalPhoto>> BySearch
            (Guid profileId, PaginationOptions options, CancellationToken cancellationToken)
        {
            return await Entities.Where(c => c.ProfileId == profileId && c.IsMainPersonalPhoto == false)
                .Skip(options.Offset * options.Take)
                .Take(options.Take)
                .ToListAsync(cancellationToken);
        }
    }
}
