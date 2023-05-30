using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.ElectionResults.Queries;

public class GetElectionTotalVotesCommand : IRequest<string>
{
    public long ElectionId { get; set; }
}
public class GetElectionTotalVotesCommandHandler : IRequestHandler<GetElectionTotalVotesCommand, string>
{
    private readonly IApplicationContext _dbContext;
    public GetElectionTotalVotesCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(GetElectionTotalVotesCommand request, CancellationToken cancellationToken)
    {
        var totalVotes = await _dbContext.Votes.Where(x => x.ElectionId == request.ElectionId).SumAsync(x => x.TotalVotes, cancellationToken);

        return totalVotes.ToString();
    }

}