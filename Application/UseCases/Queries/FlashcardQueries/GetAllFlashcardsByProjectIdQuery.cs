using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.FlashcardQueries;

public class GetAllFlashcardsByProjectIdQuery : IRequest<Result<FlashcardsResponse>>
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
}