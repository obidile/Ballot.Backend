using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.ElectionResults.Queries;

public class GetCandidateTotalVotes : IRequest<long>
{
    public long ElectionId { get; set; }
    public long CandidateId { get; set; }
}

public class GetCandidateTotalVotesHandler : IRequestHandler<GetCandidateTotalVotes, long>
{
    private readonly IApplicationContext _dbContext;

    public GetCandidateTotalVotesHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> Handle(GetCandidateTotalVotes request, CancellationToken cancellationToken)
    {
        var totalVotes = await _dbContext.Votes.Where(x => x.ElectionId == request.ElectionId && x.CandidateId == request.CandidateId)
            .SumAsync(x => x.TotalVotes, cancellationToken);

        return totalVotes;
    }
}