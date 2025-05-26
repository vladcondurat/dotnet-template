using Application.DTOs;
using Application.UseCases.Commands.UserCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}