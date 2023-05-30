using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.ElectionResults.Queries;

public class GetElectionWinner : IRequest<string>
{
    public long ElectionId { get; set; }
}
public class GetElectionWinnerHandler : IRequestHandler<GetElectionWinner, string>
{
    private readonly IApplicationContext _dbContext;
    public GetElectionWinnerHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(GetElectionWinner request, CancellationToken cancellationToken)
    {
        var candidateResults = await _dbContext.Votes.Where(x => x.ElectionId == request.ElectionId).GroupBy(x => x.CandidateId)
        .Select(x => new { CandidateId = x.Key, TotalVotes = x.Sum(x => x.TotalVotes) }).OrderByDescending(x => x.TotalVotes) 
        .FirstOrDefaultAsync(cancellationToken);

        if (candidateResults == null)
        {
            return "No winner yet";
        }

        var winnerCandidate = await _dbContext.Candidates.Where(x => x.Id == candidateResults.CandidateId).Select(x => x.Name)
            .FirstOrDefaultAsync(cancellationToken);

        return $"{winnerCandidate} is the winner of the election";
    }
}