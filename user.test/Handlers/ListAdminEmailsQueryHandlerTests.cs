using Microsoft.Extensions.Logging;
using Moq;
using user.application.User;
using user.handler.User.v1;
using user.request.Querys.v1.User;

namespace user.test.Handlers;
[TestFixture]
public class ListAdminEmailsQueryHandlerTests
{
    private Mock<ILogger<ListAdminEmailsQueryHandler>> _loggerMock;
    private Mock<IUserService> _userServiceMock;
    private ListAdminEmailsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<ListAdminEmailsQueryHandler>>();
        _userServiceMock = new Mock<IUserService>();
        _handler = new ListAdminEmailsQueryHandler(_loggerMock.Object, _userServiceMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessResponse_WithAdminEmails()
    {
        var adminEmails = new List<string> { "admin1@example.com", "admin2@example.com" };
        _userServiceMock.Setup(s => s.GetAdminEmailsAsync(default)).ReturnsAsync(adminEmails);

        var result = await _handler.Handle(new ListAdminEmailsQuery(), default);

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(2, result.Data.Count());
        Assert.AreEqual("admin1@example.com", result.Data.First());
    }
}
