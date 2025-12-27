using Application.Features.Tests.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Tests.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTestCommand, Test>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<Test, CreateTestResponseDTO>();
        }
    }
}
