#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Requests
{
    public class PetPhotoRequest
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public Guid PetId { get; init; }
        [Required]
        public Guid AccountId { get; init; }
    }
}
