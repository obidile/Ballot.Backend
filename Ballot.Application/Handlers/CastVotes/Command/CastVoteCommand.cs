using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.CastVotes.Command;

public class CastVoteCommand : IRequest<string>
{
    public int ElectionId { get; set; }
    public int UserId { get; set; }
    public int CandidateId { get; set; }
}

public class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public CastVoteCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(CastVoteCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.FindAsync(request.ElectionId);
        var user = await _dbContext.Users.FindAsync(request.UserId);
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (election == null || user == null || candidate == null)
        {
            return "Invalid input data.";
        }
        if (candidate.IsApproved == false)
        {
            return "Invalid candidate selection";
        }

        if (candidate.Disqualified == true)
        {
            return "This candidate has been disqulified from this election";
        }

        if (election.EndDate < DateTime.UtcNow)
        {
            return "This election has Ended";
        }
        else if (election.StartDate > DateTime.UtcNow)
        {
            return "This election has not started";
        }

        bool hasVoted = await _dbContext.CastVotes.AnyAsync(x => x.Id == request.ElectionId && x.Id == request.UserId, cancellationToken);

        if (hasVoted)
        {
            return "You have casted your vote in this election.";
        }

        // Get candidate and update vote count
        var candidateResult = await _dbContext.CandidateResults.FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId, cancellationToken);

        if (candidateResult == null)
        {
            return "Invalid input data.";
        }

        candidateResult.TotalVotes = (int.Parse(candidateResult.TotalVotes) + 1).ToString();

        var castVote = new CastVote
        {
            User = user,
            Election = election,
            Candidate = candidate
        };


        _dbContext.CastVotes.Add(castVote);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return $"You have voted for {candidate.Name} in the {election} elections";
    }
}

