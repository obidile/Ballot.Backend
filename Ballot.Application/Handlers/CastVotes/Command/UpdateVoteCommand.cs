using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.CastVotes.Command;

public class UpdateVoteCommand : IRequest<string>
{
    public long ElectionId { get; set; }
    public long CandidateId { get; set; }
    public long UserId { get; set; }
}
public class UpdateVoteCommandHandler : IRequestHandler<UpdateVoteCommand, string>
{
    private readonly IApplicationContext _dbContext;
    public UpdateVoteCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
    {
        var castVote = await _dbContext.CastVotes.FirstOrDefaultAsync(x => x.Election.Id == request.ElectionId && x.User.Id == request.UserId, cancellationToken);

        if (castVote == null)
        {
            return "Vote not found";
        }

        var oldCandidateId = castVote.Candidate.Id;
        var newCandidateId = request.CandidateId;

        if (oldCandidateId == newCandidateId)
        {
            return "You have already voted for this candidate";
        }

        var election = await _dbContext.Elections.FindAsync(request.ElectionId);
        var user = await _dbContext.Users.FindAsync(request.UserId);
        var oldCandidate = await _dbContext.Candidates.FindAsync(oldCandidateId);
        var newCandidate = await _dbContext.Candidates.FindAsync(newCandidateId);
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);


        if (election == null || user == null || oldCandidate == null || newCandidate == null)
        {
            return "Invalid input data";
        }
        if (election.EndDate < DateTime.UtcNow)
        {
            return "This election has ended";
        }

        if (candidate.IsApproved == false)
        {
            return "Invalid candidate selection";
        }

        if (candidate.Disqualified == true)
        {
            return "This candidate has been disqulified from this election";
        }
        else if (election.StartDate > DateTime.UtcNow)
        {
            return "This election has not started";
        }

        castVote.Candidate = newCandidate;

        // Get old candidate and update vote count
        var oldCandidateResult = await _dbContext.CandidateResults.FirstOrDefaultAsync(x => x.CandidateId == oldCandidateId, cancellationToken);
        if (oldCandidateResult != null)
        {
            oldCandidateResult.TotalVotes = (int.Parse(oldCandidateResult.TotalVotes) - 1).ToString();
        }

        // Get new candidate and update vote count
        var newCandidateResult = await _dbContext.CandidateResults.FirstOrDefaultAsync(x => x.CandidateId == newCandidateId, cancellationToken);
        if (newCandidateResult != null)
        {
            newCandidateResult.TotalVotes = (int.Parse(newCandidateResult.TotalVotes) + 1).ToString();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return $"You have updated your vote from {oldCandidate.Name} to {newCandidate.Name} in the {election.Name} election.";
    }
}
