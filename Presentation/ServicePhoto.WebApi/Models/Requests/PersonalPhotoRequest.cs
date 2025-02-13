#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Requests
{
    public class PersonalPhotoRequest
    {
        [Required]
        public Guid PhotoId { get; set; }
        [Required]
        public Guid ProfileId { get; init; }
    }
}
