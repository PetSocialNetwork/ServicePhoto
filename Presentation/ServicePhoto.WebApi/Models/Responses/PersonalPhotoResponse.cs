#pragma warning disable CS8618 
using System.ComponentModel.DataAnnotations;

namespace ServicePhoto.WebApi.Models.Responses
{
    public class PersonalPhotoResponse
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public Guid ProfileId { get; init; }
        [Required]
        public bool IsMainPersonalPhoto { get; set; }
    }
}
