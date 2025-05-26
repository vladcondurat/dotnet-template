using AutoMapper;
using Application.DTOs;
using Application.UseCases.Commands.FlashcardCommands;
using Domain.Entities;

namespace Application.Mappings;

public class FlashcardMappingProfile : Profile
{
    public FlashcardMappingProfile()
    {
        CreateMap<Flashcard, FlashcardDto>();
        CreateMap<GenerateFlashcardsRequest, GenerateFlashcardsCommand>();
    }
}