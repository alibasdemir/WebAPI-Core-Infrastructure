using Application.Features.Auth.Commands.Register;
using Application.Features.Tests.Commands.Create;
using AutoMapper;
using Core.Security.Entities;

namespace Application.Features.Auth.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterCommand, User>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, RegisterResponseDTO>();
        }
    }
}
