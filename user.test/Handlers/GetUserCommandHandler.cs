using Microsoft.Extensions.Logging;
using Moq;
using user.application.User;
using user.entities;
using user.handler.User.v1;
using user.internalservices.Helpers;
using user.request.Commands.v1;

namespace user.test.Handlers;

[TestFixture]
public class GetUserCommandHandlerTests
{
    private Mock<ILogger<GetUserCommandHandler>> _loggerMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<IUserService> _userServiceMock;
    private GetUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<GetUserCommandHandler>>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userServiceMock = new Mock<IUserService>();
        _handler = new GetUserCommandHandler(
            _loggerMock.Object,
            _passwordHasherMock.Object,
            _userServiceMock.Object
        );
    }

    [Test]
    public async Task Handle_ReturnsSuccessResponse_WhenCredentialsAreValid()
    {
        var command = new GetUserCommand { Username = "testuser", Password = "password123" };
        var userEntity = new User
        {
            Username = "testuser",
            HashedPassword = "hashedPassword",
            Salt = "salt"
        };

        _userServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), default))
                        .ReturnsAsync(userEntity);
        _passwordHasherMock.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(true);

        var result = await _handler.Handle(command, default);

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("testuser", result.Data.Username);
    }
}