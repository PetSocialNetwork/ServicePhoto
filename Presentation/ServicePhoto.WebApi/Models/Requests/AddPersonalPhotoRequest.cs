#pragma warning disable CS8618 

namespace ServicePhoto.WebApi.Models.Requests
{
    public class AddPersonalPhotoRequest
    {
        public Guid ProfileId { get; set; }
        public byte[] FileBytes { get; set; }
        public string OriginalFileName { get; set; }
    }
}
