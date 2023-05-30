using Ballot.Application.Handlers.CastVotes.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ballot.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CastVotesController : ControllerBase
{
     private readonly IMediator _mediator;
	public CastVotesController(IMediator mediator)
	{
		_mediator = mediator;
	}

    [HttpPost]
    public async Task<IActionResult> CastVote(CastVoteCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{electionId}/{userId}")]
    public async Task<IActionResult> UpdateVote(long electionId, long userId, [FromBody] UpdateVoteCommand command)
    {
        command.ElectionId = electionId;
        command.UserId = userId;

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{electionId}/{userId}/{candidateId}")]
    public async Task<IActionResult> Delete(long electionId, long userId, long candidateId)
    {
        var command = new DeleteVoteCommand { ElectionId = electionId, UserId = userId, CandidateId = candidateId };
        var result = await _mediator.Send(command);

        return Ok(result);
    }

}
