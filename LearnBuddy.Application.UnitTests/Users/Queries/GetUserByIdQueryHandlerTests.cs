using Application.DTOs;
using Application.UseCases.Queries.UserQueries;
using Application.UseCases.QueryHandlers.UserQueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace SmartRealEstateManagement.Application.UnitTests.Users.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserByIdQueryHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Given_ExistingUserId_When_HandleIsCalled_Then_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = "testuser",
        };

        _unitOfWork.Users.FindAsync(userId)!.Returns(Task.FromResult(user));

        var userDto = new UserDto
        {
            Id = userId,
            Username = user.Username,
        };
        _mapper.Map<UserDto>(user).Returns(userDto);

        var query = new GetUserByIdQuery { Id = userId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDto);
        await _unitOfWork.Users.Received(1).FindAsync(userId);
    }

    [Fact]
    public async Task Given_NonExistentUserId_When_HandleIsCalled_Then_ShouldReturnEntityNotFoundError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        _unitOfWork.Users.FindAsync(nonExistentUserId)!.Returns(Task.FromResult<User>(null!));

        var query = new GetUserByIdQuery { Id = nonExistentUserId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("EntityNotFound");
        result.Error.Message.Should().Be($"Entity 'User' not found for ID {nonExistentUserId}.");
        result.Error.EntityId.Should().Be(nonExistentUserId);
        result.Error.EntityType.Should().Be(typeof(User));

        await _unitOfWork.Users.Received(1).FindAsync(nonExistentUserId);
    }
}