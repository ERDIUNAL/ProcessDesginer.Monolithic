using Application.Features.Auth.Commands.Revoke;
using AutoMapper;
using Crea.Core.Application.Dtos;
using Crea.Core.Security.Entities;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserForRegisterDto>().ReverseMap();
        CreateMap<RefreshToken,RevokedResponse>().ReverseMap();
    }
}
