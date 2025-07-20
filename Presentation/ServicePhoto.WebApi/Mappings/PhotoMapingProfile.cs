using AutoMapper;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Shared;
using ServicePhoto.WebApi.Models.Requests;
using ServicePhoto.WebApi.Models.Responses;

namespace ServicePhoto.WebApi.Mappings
{
    public class PhotoMapingProfile : Profile
    {
        public PhotoMapingProfile()
        {
            CreateMap<PetPhoto, PetPhotoReponse>();
            CreateMap<PersonalPhoto, PersonalPhotoResponse>();
            CreateMap<AddPersonalPhotoRequest, PersonalPhoto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId))
                .ForMember(dest => dest.FileBytes, opt => opt.MapFrom(src => src.FileBytes))
                .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => src.OriginalFileName));

            CreateMap<AddPetPhotoRequest, PetPhoto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.PetId, opt => opt.MapFrom(src => src.PetId))
               .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId))
               .ForMember(dest => dest.FileBytes, opt => opt.MapFrom(src => src.FileBytes))
               .ForMember(dest => dest.OriginalFileName, opt => opt.MapFrom(src => src.OriginalFileName));

            CreateMap<PaginationRequest, PaginationOptions>();
        }
    }
}
