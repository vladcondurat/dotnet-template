using Application.Use_Classes.CommandHandlers.UserCommandHandlers;
using Application.Use_Classes.Commands.UserCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace SmartRealEstateManagement.Application.UnitTests.Users.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateUserCommandHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Given_ExistingUser_When_HandleIsCalled_Then_UserShouldBeUpdatedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            Username = "existinguser",
        };

        _unitOfWork.Users.FindAsync(userId)!.Returns(Task.FromResult(existingUser));

        var command = new UpdateUserCommand
        {
            Id = userId,
            Username = "updateduser",
        };

        _mapper.When(m => m.Map(command, existingUser)).Do(_ =>
        {
            existingUser.Username = command.Username;
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWork.Users.Received(1).Update(existingUser);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Given_NonExistentUser_When_HandleIsCalled_Then_ShouldReturnEntityNotFoundError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        _unitOfWork.Users.FindAsync(nonExistentUserId)!.Returns(Task.FromResult<User>(null!));

        var command = new UpdateUserCommand
        {
            Id = nonExistentUserId,
            Username = "nonexistentuser",
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("EntityNotFound");
        result.Error.Message.Should().Be($"Entity 'User' not found for ID {nonExistentUserId}.");
        result.Error.EntityId.Should().Be(nonExistentUserId);
        result.Error.EntityType.Should().Be(typeof(User));

        _unitOfWork.Users.DidNotReceive().Update(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
