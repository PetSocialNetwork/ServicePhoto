#pragma warning disable CS8618
namespace ServicePhoto.WebApi.Models.Requests
{
    public class PetPhotoBySearchRequest
    {
        public Guid PetId { get; set; }
        public Guid ProfileId { get; set; }
        public PaginationRequest Options { get; set; }
    }
}
