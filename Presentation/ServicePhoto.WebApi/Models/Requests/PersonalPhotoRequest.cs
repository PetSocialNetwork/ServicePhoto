#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Requests
{
    public class PersonalPhotoRequest
    {
        [Required]
        public string Path { get; set; }
        [Required]
        public Guid AccountId { get; init; }
    }
}
