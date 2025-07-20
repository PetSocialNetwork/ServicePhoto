namespace ServicePhoto.WebApi.Models.Requests
{
    public class PetMainPhotoRequest
    {
        public Guid PetId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
