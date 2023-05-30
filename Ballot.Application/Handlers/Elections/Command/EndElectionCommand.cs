using AutoMapper;
using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Elections.Command;

public class EndElectionCommand : IRequest<string>
{
    public long ElectionId { get; set; }
}
public class EndElectionCommandHandler : IRequestHandler<EndElectionCommand, string>
{
    private readonly IApplicationContext _dbContext;
    public EndElectionCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(EndElectionCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.SingleOrDefaultAsync(x => x.Id == request.ElectionId, cancellationToken);

        if (election == null)
        {
            return "No election with the specified ID was found";
        }

        if (election.EndDate < DateTime.UtcNow)
        {
            return "The election has already ended.";
        }

        election.EndDate = DateTime.UtcNow;
        election.IsActive = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"{election.Name} has been ended";
    }
}