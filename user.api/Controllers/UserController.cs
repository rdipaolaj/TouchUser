using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using user.api.Configuration;
using user.request.Commands.v1;
using user.request.Querys.v1.User;

namespace user.api.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("touch/user/api/v{v:apiVersion}/[controller]")]
public class UserController : CustomController
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [MapToApiVersion(1)]
    [Route("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        _logger.LogInformation("Creating user...");
        var result = await _mediator.Send(command);
        return OkorBadRequestValidationApiResponse(result);
    }

    [HttpPost]
    [Route("get-user")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetUserByUsernameAndPassword([FromBody] GetUserCommand command)
    {
        _logger.LogInformation("Getting user...");
        var result = await _mediator.Send(command);
        return OkorBadRequestValidationApiResponse(result);
    }

    [HttpGet]
    [Route("admin-emails")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAdminEmails()
    {
        _logger.LogInformation("Fetching admin emails...");
        var result = await _mediator.Send(new ListAdminEmailsQuery());
        return OkorBadRequestValidationApiResponse(result);
    }
}
