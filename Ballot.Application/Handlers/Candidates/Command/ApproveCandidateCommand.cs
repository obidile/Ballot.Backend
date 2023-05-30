using Ballot.Application.Common.Interface;
using MediatR;

namespace Ballot.Application.Handlers.Candidates.Command;

public class ApproveCandidateCommand : IRequest<string>
{
    public long CandidateId { get; set; }
}
public class ApproveCandidateCommandHandler : IRequestHandler<ApproveCandidateCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public ApproveCandidateCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(ApproveCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (candidate == null)
        {
            return "Candidate not found.";
        }

        if (candidate.IsApproved == true)
        {
            return "Candidate has already been approved.";
        }

        candidate.IsApproved = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"Candidate {candidate.Name} has been approved.";
    }
}