using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Candidates.Queries;

public class GetDisqualifiedCandidatesOfElectionQuery : IRequest<List<CandidateModel>>
{
    public long ElectionId { get; set; }
}
public class GetDisqualifiedCandidatesOfElectionQueryHandler : IRequestHandler<GetDisqualifiedCandidatesOfElectionQuery, List<CandidateModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetDisqualifiedCandidatesOfElectionQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CandidateModel>> Handle(GetDisqualifiedCandidatesOfElectionQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _dbContext.Candidates.Where(x => x.ElectionId == request.ElectionId && x.Disqualified == true).ToListAsync(cancellationToken);

        var result = _mapper.Map<List<CandidateModel>>(candidates);
        return result;
    }
}
