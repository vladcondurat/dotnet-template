using Application.DTOs;
using Application.Use_Classes.Queries.UserQueries;
using Application.Use_Classes.QueryHandlers.UserQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace SmartRealEstateManagement.Application.UnitTests.Users.Queries;

public class GetAllUsersQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GetAllUsersQueryHandler _handler;

    public GetAllUsersQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetAllUsersQueryHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Given_UsersExist_When_HandleIsCalled_Then_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "user1" },
            new User { Id = Guid.NewGuid(), Username = "user2" }
        };
        _unitOfWork.Users.GetAllAsync().Returns(Task.FromResult((IEnumerable<User>)users));

        var userDtos = new List<UserDto>
        {
            new UserDto { Id = users[0].Id, Username = users[0].Username },
            new UserDto { Id = users[1].Id, Username = users[1].Username }
        };
        _mapper.Map<IEnumerable<UserDto>>(users).Returns(userDtos);

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDtos);
        await _unitOfWork.Users.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task Given_NoUsersExist_When_HandleIsCalled_Then_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWork.Users.GetAllAsync().Returns(Task.FromResult<IEnumerable<User>>(new List<User>()));

        _mapper.Map<IEnumerable<UserDto>>(Arg.Any<IEnumerable<User>>()).Returns(new List<UserDto>());

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        await _unitOfWork.Users.Received(1).GetAllAsync();
    }
}