using Microsoft.Extensions.Logging;
using Moq;
using user.application.Rol;
using user.application.User;
using user.common.Responses;
using user.entities;
using user.handler.User.v1;
using user.internalservices.Helpers;
using user.redis.Users;
using user.request.Commands.v1;

namespace user.test.Handlers;

[TestFixture]
public class CreateUserCommandHandlerTests
{
    private Mock<ILogger<CreateUserCommandHandler>> _loggerMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<IRoleValidationService> _roleValidationMock;
    private Mock<IUserValidationService> _userValidationMock;
    private Mock<IUserService> _userServiceMock;
    private Mock<IRedisUserService> _redisUserServiceMock;
    private CreateUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<CreateUserCommandHandler>>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _roleValidationMock = new Mock<IRoleValidationService>();
        _userValidationMock = new Mock<IUserValidationService>();
        _userServiceMock = new Mock<IUserService>();
        _redisUserServiceMock = new Mock<IRedisUserService>();
        _handler = new CreateUserCommandHandler(
            _loggerMock.Object,
            _passwordHasherMock.Object,
            _roleValidationMock.Object,
            _userValidationMock.Object,
            _userServiceMock.Object,
            _redisUserServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_ReturnsSuccessResponse_WhenUserIsCreated()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "prueba",
            Password = "1234",
            UserRole = 1
        };

        var hashedPassword = ("hashedPassword", "salt");
        var userResponse = new User { UserId = Guid.NewGuid() };

        _userValidationMock.Setup(s => s.ValidateUserAsync(It.IsAny<CreateUserCommand>(), default))
                           .ReturnsAsync(ApiResponseHelper.CreateSuccessResponse());
        _roleValidationMock.Setup(s => s.ValidateRoleAsync(It.IsAny<int>(), default))
                           .ReturnsAsync(ApiResponseHelper.CreateSuccessResponse(new Role { RoleId = Guid.NewGuid() }));
        _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<string>()))
                           .Returns(hashedPassword);
        _userServiceMock.Setup(s => s.CreateUserAsync(
            It.IsAny<CreateUserCommand>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<Guid>(),
            default))
                        .ReturnsAsync(userResponse);

        var result = await _handler.Handle(command, default);

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
    }
}