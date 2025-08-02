#pragma warning disable CS8618
namespace ServicePhoto.WebApi.Models.Requests
{
    public class PersonalPhotoBySearchRequest
    {
        public Guid ProfileId { get; set; }
        public PaginationRequest Options { get; set; }
    }
}
