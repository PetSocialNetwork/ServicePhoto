using ServicePhoto.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePhoto.Domain.Entities
{
    public class PersonalPhoto : IEntity
    {
        public Guid Id { get; init; }
        public string FilePath { get; set; }
        public Guid ProfileId { get; init; }
        public bool IsMainPersonalPhoto { get; set; }
        [NotMapped]
        public byte[] FileBytes { get; set; }
        [NotMapped]
        public string OriginalFileName { get; set; }
        public PersonalPhoto(Guid id, Guid profileId, string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            Id = id;
            ProfileId = profileId;
            FilePath = filePath;
        }
        protected PersonalPhoto() { }
    }
}
