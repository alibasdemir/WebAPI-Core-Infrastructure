using Application.Features.Auth.Commands.ChangePassword;
using Application.Features.Auth.Commands.DeleteUser;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.SoftDeleteUser;
using Application.Features.Auth.Commands.UpdateUser;
using Application.Features.Auth.Queries.GetByIdUser;
using Application.Features.Auth.Queries.GetCurrentUser;
using Application.Features.Auth.Queries.GetListUser;
using Application.Features.Auth.Queries.SearchUsers;
using AutoMapper;
using Core.Pagination;
using Core.Security.Entities;
using Core.Security.JWT;

namespace Application.Features.Auth.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Register mapping
            CreateMap<RegisterCommand, User>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Login mapping
            CreateMap<User, RegisterResponseDTO>();

            CreateMap<AccessToken, LoginResponseDTO>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src));

            // UpdateUser mapping
            CreateMap<User, UpdateUserResponseDTO>();

            // DeleteUser mapping
            CreateMap<User, DeleteUserResponseDTO>();

            // SoftDeleteUser mapping
            CreateMap<User, SoftDeleteUserResponseDTO>()
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate ?? DateTime.UtcNow));

            // ChangePassword mapping
            CreateMap<User, ChangePasswordResponseDTO>();

            // GetById mapping
            CreateMap<User, GetByIdUserResponseDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles separately mapped in handler

            // GetCurrent mapping
            CreateMap<User, GetCurrentUserResponseDTO>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles separately mapped in handler

            // GetList mapping
            CreateMap<IPaginate<User>, GetListUserResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));

            CreateMap<User, UserItemDTO>();

            // Search mapping
            CreateMap<IPaginate<User>, SearchUsersResponseDTO>()
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext));

            CreateMap<User, SearchUserItemDTO>();
        }
    }
}
