using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user.api.Configuration;
using user.request.Querys.v1.Rol;

namespace user.api.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("touch/role/api/v{v:apiVersion}/[controller]")]
public class RoleController : CustomController
{
    private readonly ILogger<RoleController> _logger;
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator, ILogger<RoleController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("list-roles")]
    public async Task<IActionResult> GetRole()
    {
        _logger.LogInformation("Getting roles...");
        var result = await _mediator.Send(new GetListRolesQuery());
        return OkorBadRequestValidationApiResponse(result);
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("validate-role")]
    public async Task<IActionResult> GetRoleById()
    {
        _logger.LogInformation("Getting roles...");
        var result = await _mediator.Send(new GetListRolesQuery());
        return OkorBadRequestValidationApiResponse(result);
    }
}
