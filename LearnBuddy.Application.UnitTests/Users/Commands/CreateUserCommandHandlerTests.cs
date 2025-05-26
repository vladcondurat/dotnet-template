using Application.Use_Classes.Commands.UserCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Application.Use_Classes.CommandHandlers.UserCommandHandlers;

namespace SmartRealEstateManagement.Application.UnitTests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateUserCommandHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Given_ValidCreateUserCommand_When_HandleIsCalled_Then_UserShouldBeCreated()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "newuser",
        };

        _unitOfWork.Users.GetByUsernameAsync(command.Username)!.Returns(Task.FromResult<User>(null!));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
        };

        _mapper.Map<User>(command).Returns(user);
        _unitOfWork.Users.AddAsync(user).Returns(Task.FromResult(Result<Guid>.Success(user.Id)));
        _unitOfWork.SaveChangesAsync().Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(user.Id);
        await _unitOfWork.Users.Received(1).AddAsync(user);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}
