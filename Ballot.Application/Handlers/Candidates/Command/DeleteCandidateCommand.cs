using Ballot.Application.Common.Interface;
using MediatR;

namespace Ballot.Application.Handlers.Candidates.Command;

public class DeleteCandidateCommand : IRequest<string>
{
    public long CandidateId { get; set; }
}
public class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public DeleteCandidateCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (candidate == null)
        {
            return "Candidate not found.";
        }

        _dbContext.Candidates.Remove(candidate);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"The candidate {candidate.Name} has been deleted.";
    }
}
