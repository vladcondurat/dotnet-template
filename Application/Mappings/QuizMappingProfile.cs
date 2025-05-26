using AutoMapper;
using Application.DTOs;
using Application.UseCases.Commands.QuizCommands;
using Domain.Entities;

namespace Application.Mappings;

public class QuizMappingProfile : Profile
{
    public QuizMappingProfile()
    {
        CreateMap<Quiz, QuizDto>();
        CreateMap<Quiz, QuizDetailDto>();
        CreateMap<QuizQuestion, QuizQuestionDto>();
        CreateMap<GenerateQuizRequest, GenerateQuizCommand>();
    }
} 