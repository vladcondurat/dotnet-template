using Application.Use_Classes.Commands.UserCommands;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Application.Use_Classes.CommandHandlers.UserCommandHandlers;

namespace SmartRealEstateManagement.Application.UnitTests.Users.Commands;

public class DeleteUserByIdCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteUserByIdCommandHandler _handler;

    public DeleteUserByIdCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteUserByIdCommandHandler(_unitOfWork);
    }

    [Fact]
    public async Task Given_ExistingUserId_When_HandleIsCalled_Then_UserShouldBeDeleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Username = "testuser" };

        _unitOfWork.Users.FindAsync(userId)!.Returns(Task.FromResult(user));

        var command = new DeleteUserByIdCommand { Id = userId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWork.Users.Received(1).Delete(user);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Given_NonExistentUserId_When_HandleIsCalled_Then_ShouldReturnEntityNotFoundError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        _unitOfWork.Users.FindAsync(nonExistentUserId)!.Returns(Task.FromResult<User>(null!));

        var command = new DeleteUserByIdCommand { Id = nonExistentUserId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("EntityNotFound");
        result.Error.Message.Should().Be($"Entity 'User' not found for ID {nonExistentUserId}.");
        result.Error.EntityId.Should().Be(nonExistentUserId);
        result.Error.EntityType.Should().Be(typeof(User));

        _unitOfWork.Users.DidNotReceive().Delete(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
    }
}
