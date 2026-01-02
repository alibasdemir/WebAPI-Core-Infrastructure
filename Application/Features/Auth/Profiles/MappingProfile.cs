using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using AutoMapper;
using Core.Security.Entities;
using Core.Security.JWT;

namespace Application.Features.Auth.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterCommand, User>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, RegisterResponseDTO>();

            CreateMap<AccessToken, LoginResponseDTO>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src));
        }
    }
}
