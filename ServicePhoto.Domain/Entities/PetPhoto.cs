using ServicePhoto.Domain.Interfaces;

namespace ServicePhoto.Domain.Entities
{
    public class PetPhoto : IEntity
    {
        public Guid Id { get; init; }
        public string Path { get; set; }
        public Guid AccountId { get; init; }
        public bool IsMainPetPhoto { get; set; }
        protected PetPhoto() { }
    }
}
