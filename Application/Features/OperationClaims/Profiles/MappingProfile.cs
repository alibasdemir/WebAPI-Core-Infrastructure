using Application.Features.OperationClaims.Commands.Create;
using Application.Features.OperationClaims.Commands.Delete;
using Application.Features.OperationClaims.Commands.Update;
using Application.Features.OperationClaims.Queries.GetById;
using Application.Features.OperationClaims.Queries.GetList;
using AutoMapper;
using Core.Pagination;
using Core.Security.Entities;

namespace Application.Features.OperationClaims.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOperationClaimCommand, OperationClaim>();
            CreateMap<OperationClaim, CreateOperationClaimResponseDTO>();

            CreateMap<OperationClaim, UpdateOperationClaimResponseDTO>();
            CreateMap<OperationClaim, DeleteOperationClaimResponseDTO>();

            CreateMap<OperationClaim, GetByIdOperationClaimResponseDTO>();

            CreateMap<IPaginate<OperationClaim>, GetListOperationClaimResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));
            CreateMap<OperationClaim, OperationClaimItemDTO>();
        }
    }
}