using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Elections.Command;

public class StartElectionCommand : IRequest<string>
{
    public long ElectionId { get; set; }
}
public class StartElectionCommandHandler : IRequestHandler<StartElectionCommand, string>
{
    private readonly IApplicationContext _dbContext;
    public StartElectionCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(StartElectionCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.SingleOrDefaultAsync(x => x.Id == request.ElectionId, cancellationToken);

        if (election == null)
        {
            return "Election not found";
        }

        if (DateTime.UtcNow >= election.StartDate)
        {
            return "Election has already started";
        }

        election.StartDate = DateTime.UtcNow;
        election.IsActive = true;

        _dbContext.Elections.Update(election);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"{election.Name} Election started";
    }
}