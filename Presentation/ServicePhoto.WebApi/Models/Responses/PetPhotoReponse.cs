#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Responses
{
    public class PetPhotoReponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public Guid AccountId { get; init; }
        [Required]
        public bool IsMainPetPhoto { get; set; }
    }
}
