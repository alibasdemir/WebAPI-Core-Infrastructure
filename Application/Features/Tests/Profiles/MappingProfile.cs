using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Delete;
using Application.Features.Tests.Commands.SoftDelete;
using Application.Features.Tests.Commands.Update;
using Application.Features.Tests.Queries.GetById;
using Application.Features.Tests.Queries.GetList;
using Application.Features.Tests.Queries.Search;
using AutoMapper;
using Core.Pagination;
using Domain.Entities;

namespace Application.Features.Tests.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Command mappings
            CreateMap<CreateTestCommand, Test>();
            CreateMap<Test, CreateTestResponseDTO>();

            CreateMap<Test, UpdateTestResponseDTO>();

            CreateMap<Test, DeleteTestResponseDTO>();

            CreateMap<Test, SoftDeleteTestResponseDTO>()
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate ?? DateTime.UtcNow));

            // Query mappings
            CreateMap<Test, GetByIdTestResponseDTO>();

            CreateMap<IPaginate<Test>, GetListTestResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));
            CreateMap<Test, TestItemDTO>();

            CreateMap<IPaginate<Test>, SearchTestsResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));
            CreateMap<Test, SearchTestItemDTO>();
        }
    }
}
