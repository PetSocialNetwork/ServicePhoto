using ServicePhoto.Domain.Interfaces;

namespace ServicePhoto.Domain.Entities
{
    public class PersonalPhoto : IEntity
    {
        public Guid Id { get; init; }
        public string FilePath { get; set; }
        public Guid AccountId { get; init; }
        public bool IsMainPersonalPhoto { get; set; }
        public PersonalPhoto(Guid id, Guid accountId, string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            Id = id;
            AccountId = accountId;
            FilePath = filePath;
        }
        protected PersonalPhoto() { }
    }
}
