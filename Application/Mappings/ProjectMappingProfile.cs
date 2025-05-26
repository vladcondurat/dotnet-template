using Application.DTOs;
using Application.UseCases.Commands.ProjectCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<UpdateProjectRequest, UpdateProjectCommand>();
        CreateMap<UpdateProjectCommand, Project>();
        CreateMap<UpdateProjectSummaryCommand, Project>();
        CreateMap<UpdateProjectSummaryRequest, UpdateProjectSummaryCommand>();
        CreateMap<CreateProjectRequest, CreateProjectCommand>();
        CreateMap<Project, ProjectContentDto>();
        CreateMap<Project, ProjectDto>();
    }
}