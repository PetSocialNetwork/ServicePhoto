using ServicePhoto.Domain.Interfaces;

namespace ServicePhoto.Domain.Entities
{
    public class PersonalPhoto : IEntity
    {
        public Guid Id { get; init; }
        public string Path { get; set; }
        public Guid AccountId { get; init; }
        public bool IsMainPersonalPhoto { get; set; }
        protected PersonalPhoto() { }
    }
}
