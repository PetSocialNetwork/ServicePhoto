using Microsoft.EntityFrameworkCore;
using ServicePhoto.Domain.Entities;

namespace ServicePhoto.DataEntityFramework
{
    public class AppDbContext : DbContext
    {
        DbSet<PetPhoto> PetPhotos => Set<PetPhoto>();
        DbSet<PersonalPhoto> PersonalPhotos => Set<PersonalPhoto>();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    }
}
