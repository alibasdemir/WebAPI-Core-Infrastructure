using Application.Features.UserOperationClaims.Commands.Assign;
using Application.Features.UserOperationClaims.Commands.Revoke;
using Application.Features.UserOperationClaims.Commands.SoftDelete;
using Application.Features.UserOperationClaims.Queries.GetList;
using AutoMapper;
using Core.Pagination;
using Core.Security.Entities;

namespace Application.Features.UserOperationClaims.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserOperationClaim, AssignOperationClaimToUserResponseDTO>();
            CreateMap<UserOperationClaim, RevokeOperationClaimFromUserResponseDTO>();
            CreateMap<UserOperationClaim, SoftDeleteUserOperationClaimResponseDTO>()
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate ?? DateTime.UtcNow));

            CreateMap<IPaginate<UserOperationClaim>, GetListUserOperationClaimResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));

            CreateMap<UserOperationClaim, UserOperationClaimListItemDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.OperationClaimName, opt => opt.MapFrom(src => src.OperationClaim.Name));
        }
    }
}