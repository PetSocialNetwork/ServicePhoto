namespace ServicePhoto.WebApi.Models.Requests
{
    public class PersonalPhotoRequest
    {
        public Guid PhotoId { get; set; }
        public Guid ProfileId { get; init; }
    }
}
