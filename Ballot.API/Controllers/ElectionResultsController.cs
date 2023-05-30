using Ballot.Application.Handlers.ElectionResults.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ballot.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ElectionResultsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ElectionResultsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("candidate/{electionId}/{candidateId}/total-votes")]
    public async Task<ActionResult> GetCandidateTotalVotes(long electionId, long candidateId)
    {
        var query = new GetCandidateTotalVotes { ElectionId = electionId, CandidateId = candidateId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{electionId}/candidate-percentages")]
    public async Task<ActionResult> GetCandidatePercentages(long electionId)
    {
        var query = new GetCandidateVotePercentagesQuery { ElectionId = electionId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("election/{electionId}/total-votes")]
    public async Task<IActionResult> GetElectionTotalVotes(long electionId)
    {
        var query = new GetElectionTotalVotesCommand { ElectionId = electionId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{electionId}/winner")]
    public async Task<IActionResult> GetElectionWinner(long electionId)
    {
        var query = new GetElectionWinner { ElectionId = electionId };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
