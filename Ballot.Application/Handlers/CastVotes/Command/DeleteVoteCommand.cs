using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.CastVotes.Command;

public class DeleteVoteCommand : IRequest<string>
{
    public long ElectionId { get; set; }
    public long UserId { get; set; }
    public long CandidateId { get; set; }
}
public class DeleteVoteCommandHandler : IRequestHandler<DeleteVoteCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public DeleteVoteCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.FindAsync(request.ElectionId);
        var user = await _dbContext.Users.FindAsync(request.UserId);
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (election == null || user == null || candidate == null)
        {
            return "Invalid input data.";
        }

        var castVote = await _dbContext.CastVotes.FirstOrDefaultAsync(
            x => x.Election.Id == request.ElectionId && x.User.Id == request.UserId && x.Candidate.Id == request.CandidateId, cancellationToken);

        if (castVote == null)
        {
            return "Vote not found.";
        }

        _dbContext.CastVotes.Remove(castVote);

        // Get candidate and update vote count
        var candidateResult = await _dbContext.CandidateResults.FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId, cancellationToken);

        if (candidateResult == null)
        {
            return "Invalid input data.";
        }

        candidateResult.TotalVotes = (int.Parse(candidateResult.TotalVotes) - 1).ToString();

        await _dbContext.SaveChangesAsync(cancellationToken);
        return $"Your vote for {candidate.Name} in the {election.Name} election has been deleted.";
    }
}