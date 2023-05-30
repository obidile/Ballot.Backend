using Ballot.Application.Common.Interface;
using MediatR;

namespace Ballot.Application.Handlers.Candidates.Command;

public class DisqualifyCandidateCommand : IRequest<string>
{
    public long CandidateId { get; set; }
}
public class DisqualifyCandidateCommandHandler : IRequestHandler<DisqualifyCandidateCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public DisqualifyCandidateCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(DisqualifyCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (candidate == null)
        {
            return "Invalid candidate ID.";
        }

        if (candidate.Disqualified == false)
        {
            return "Candidate is already disqualified.";
        }

        candidate.Disqualified = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"Candidate {candidate.Name} has been disqualified.";
    }
}