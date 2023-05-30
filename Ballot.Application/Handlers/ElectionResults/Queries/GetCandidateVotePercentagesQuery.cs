using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.ElectionResults.Queries;

public class GetCandidateVotePercentagesQuery : IRequest<List<CandidatePercentage>>
{
    public long ElectionId { get; set; }
}

public class CandidatePercentage
{
    public long CandidateId { get; set; }
    public string CandidateName { get; set; }
    public double VotePercentage { get; set; }
}

public class GetCandidateVotePercentagesQueryHandler : IRequestHandler<GetCandidateVotePercentagesQuery, List<CandidatePercentage>>
{
    private readonly IApplicationContext _dbContext;

    public GetCandidateVotePercentagesQueryHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CandidatePercentage>> Handle(GetCandidateVotePercentagesQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _dbContext.Candidates.ToListAsync(cancellationToken);
        var candidatePercentages = new List<CandidatePercentage>();

        var votes = await _dbContext.Votes.Where(v => v.ElectionId == request.ElectionId).ToListAsync(cancellationToken);

        var totalVotes = votes.Sum(v => v.TotalVotes);

        foreach (var candidate in candidates)
        {
            var candidateVotes = votes.Where(v => v.CandidateId == candidate.Id).Sum(v => v.TotalVotes);
            var votePercentage = totalVotes == 0 ? 0 : (double)candidateVotes / totalVotes * 100;

            var candidatePercentage = new CandidatePercentage
            {
                CandidateId = candidate.Id,
                CandidateName = candidate.Name,
                VotePercentage = votePercentage
            };

            candidatePercentages.Add(candidatePercentage);
        }

        return candidatePercentages;
    }
}

