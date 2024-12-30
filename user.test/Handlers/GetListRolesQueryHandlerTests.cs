using Microsoft.Extensions.Logging;
using Moq;
using user.application.Rol;
using user.common.Enums;
using user.entities;
using user.handler.Rol.v1;
using user.request.Querys.v1.Rol;

namespace user.test.Handlers;

[TestFixture]
public class GetListRolesQueryHandlerTests
{
    private Mock<ILogger<GetListRolesQueryHandler>> _loggerMock;
    private Mock<IRoleServie> _roleServiceMock;
    private GetListRolesQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<GetListRolesQueryHandler>>();
        _roleServiceMock = new Mock<IRoleServie>();
        _handler = new GetListRolesQueryHandler(_loggerMock.Object, _roleServiceMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessResponse_WithRoles()
    {
        var roles = new List<Role>
        {
            new Role { RoleId = Guid.NewGuid(), RoleName = "Administrador", userRole = UserRole.Administrator },
            new Role { RoleId = Guid.NewGuid(), RoleName = "Empleado", userRole = UserRole.Employee }
        };
        _roleServiceMock.Setup(s => s.ListRolesAsync(default)).ReturnsAsync(roles);

        var result = await _handler.Handle(new GetListRolesQuery(), default);

        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(2, result.Data.Count);
        Assert.AreEqual("Administrador", result.Data.First().RoleName);
    }
}