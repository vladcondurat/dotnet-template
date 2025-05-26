using AutoMapper;
using Application.DTOs.Auth;
using Application.DTOs.Auth.Requests;
using Application.UseCases.Commands.AuthCommands;

namespace Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterUserRequestDto, RegisterCommand>();
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();
        CreateMap<ForgotPasswordRequestDto, ForgotPasswordCommand>();
        CreateMap<ConfirmEmailRequestDto, ConfirmEmailCommand>();
        CreateMap<ForgotPasswordRequestDto, ResetPasswordCommand>();
        CreateMap<ResetPasswordRequestDto, ResetPasswordCommand>();
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterUserRequestDto, RegisterCommand>();
        CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>();
        CreateMap<ForgotPasswordRequestDto, ForgotPasswordCommand>();
        CreateMap<ConfirmEmailRequestDto, ConfirmEmailCommand>();
        CreateMap<ForgotPasswordRequestDto, ResetPasswordCommand>();
        CreateMap<ResetPasswordRequestDto, ResetPasswordCommand>();
    }
}
