using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Requests
{
    public class PetMainPhotoRequest
    {
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        public Guid PhotoId { get; set; }
    }
}
