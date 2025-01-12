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

            CreateMap<PersonalPhotoRequest, PersonalPhoto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
               .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
               .ForMember(dest => dest.IsMainPersonalPhoto, opt => opt.MapFrom(src => false));

            CreateMap<PersonalPhoto,PersonalPhotoResponse>();

        }
    }
}
