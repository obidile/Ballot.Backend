using Ballot.Application.Common.Models;
using Ballot.Application.Handlers.Candidates.Command;
using Ballot.Application.Handlers.Candidates.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ballot.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly IMediator _mediator;
	public CandidatesController(IMediator mediator)
	{
		_mediator = mediator;
	}

    [HttpPost]
    public async Task<IActionResult> CreateCandidate(RegisterCandidateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCandidate(long id, UpdateCandidateCommand command)
    {
        command.CandidateId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveCandidate(long id)
    {
        var command = new ApproveCandidateCommand { CandidateId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("disqualify/{id}")]
    public async Task<IActionResult> DisqualifyCandidate(long id)
    {
        var command = new DisqualifyCandidateCommand { CandidateId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{electionId}/approved")]
    public async Task<IActionResult> GetApprovedCandidatesOfElection(long electionId)
    {
        var query = new GetCandidatesOfElectionQuery { ElectionId = electionId };
        var candidates = await _mediator.Send(query);
        return Ok(candidates);
    }

    [HttpGet("{electionId}/disqualified")]
    public async Task<IActionResult> GetDisqualifiedCandidatesOfElection(long electionId)
    {
        var query = new GetDisqualifiedCandidatesOfElectionQuery { ElectionId = electionId };
        var candidates = await _mediator.Send(query);
        return Ok(candidates);
    }


    [HttpGet("{searchQuery}")]
    public async Task<IActionResult> GetCandidateByNameOrEmail(string searchQuery)
    {
        var query = new GetCandidateBySearchQueryQuery { SearchQuery = searchQuery };
        var candidate = await _mediator.Send(query);
        return Ok(candidate);
    }

    //[HttpPut("approve/{candidateId}")]
    //public async Task<IActionResult> UpdateCandidate(long candidateId)
    //{
    //    var command = new ApproveCandidateCommand { CandidateId = candidateId };
    //    var result = await _mediator.Send(command);
    //    return Ok(result);
    //}

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCandidate(long id)
    {
        var command = new DeleteCandidateCommand { CandidateId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
