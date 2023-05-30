using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Candidates.Queries;

public class GetUnapprovedCandidatesOfElectionQuery : IRequest<List<CandidateModel>>
{
    public int ElectionId { get; set; }
}
public class GetUnapprovedCandidatesOfElectionQueryHandler : IRequestHandler<GetUnapprovedCandidatesOfElectionQuery, List<CandidateModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetUnapprovedCandidatesOfElectionQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CandidateModel>> Handle(GetUnapprovedCandidatesOfElectionQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _dbContext.Candidates.Where(x => x.ElectionId == request.ElectionId && x.IsApproved == false).ToListAsync(cancellationToken);

        var result = _mapper.Map<List<CandidateModel>>(candidates);
        return result;
    }
}