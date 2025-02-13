using AutoMapper;
using ServicePhoto.Domain.Entities;
using ServicePhoto.WebApi.Models.Requests;
using ServicePhoto.WebApi.Models.Responses;

namespace ServicePhoto.WebApi.Mappings
{
    public class PhotoMapingProfile : Profile
    {
        public PhotoMapingProfile()
        {
            CreateMap<PetPhoto, PetPhotoReponse>();
            CreateMap<PersonalPhoto,PersonalPhotoResponse>();
        }
    }
}
