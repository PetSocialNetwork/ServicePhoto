using ServicePhoto.Domain.Interfaces;

namespace ServicePhoto.Domain.Entities
{
    public class PetPhoto : IEntity
    {
        public Guid Id { get; init; }
        public string FilePath { get; set; }
        public Guid PetId { get; init; }
        public Guid AccountId { get; init; }
        public bool IsMainPetPhoto { get; set; } = false;
        public PetPhoto(Guid id, string filePath, Guid petId, Guid accountId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            Id = id;
            PetId = petId;
            AccountId = accountId;
            FilePath = filePath;
        }
        protected PetPhoto() { }
    }
}
