using Ballot.Application.Common.Models;
using Ballot.Application.Handlers.Countries.Command;
using Ballot.Application.Handlers.Countries.Quries;
using Ballot.Application.Handlers.Elections.Command;
using Ballot.Application.Handlers.Elections.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ballot.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ElectionsController : ControllerBase
{
    private readonly IMediator mediator;
	public ElectionsController(IMediator mediator)
	{
		this.mediator = mediator;
	}
    [HttpPost]
    public async Task<IActionResult> CreateElection([FromBody] CreateElectionCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("end-election/{electionId}")]
    public async Task<IActionResult> EndElection(long electionId)
    {
        var command = new EndElectionCommand { ElectionId = electionId };
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("start-election/{electionId}")]
    public async Task<IActionResult> StartElection(long electionId)
    {
        var command = new StartElectionCommand { ElectionId = electionId };
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateElection( long Id, [FromBody] UpdateElectionCommand command)
    {
         command.ElectionId = Id; 

        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveElections()
    {
        var query = new GetActiveElectionsQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetElectionByIdQuery(id)));
    }

    [HttpGet]
    public async Task<IActionResult> GetElections([FromQuery] string name)
    {
        var query = new GetElectionsQuery { Name = name };
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("ended")]
    public async Task<ActionResult> GetEndedElections()
    {
        var query = new GetEndedElectionsQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteElectionCommand { Id = Id });

        return Ok("Removed Successfully");
    }
}
