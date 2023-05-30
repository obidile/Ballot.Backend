using Ballot.Application.Common.Models;
using Ballot.Application.Handlers.Elections.Command;
using Ballot.Application.Handlers.Users.Command;
using Ballot.Application.Handlers.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ballot.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("{id}/ChangeUserStatus")]
    public async Task<IActionResult> ChangeUserStatus(long id, ChangeUserStatusCommand command)
    {
        command.UserId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(long Id, [FromBody] UpdateUserCommand command)
    {
        command.Id = Id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("Active-Inactive")]
    public async Task<ActionResult> GetActiveOrInactiveUsers(bool isActive)
    {
        var query = new GetActiveOrInactiveUsersQuery { IsActive = isActive };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(long id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { UserId = id });

        return Ok(result);
    }

    [HttpGet("AllUsers")]
    public async Task<ActionResult> GetUsers(string searchQuery, bool includeAdmins = true)
    {
        var query = new GetUsersQuery { SearchQuery = searchQuery, IncludeAdmins = includeAdmins };
        var users = await _mediator.Send(query);

        return Ok(users);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(long userId)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserId = userId });

        return Ok(result);
    }
}
