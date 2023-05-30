using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Candidates.Queries;

public class GetCandidatesOfElectionQuery : IRequest<List<CandidateModel>>
{
    public long ElectionId { get; set; }
}
public class GetCandidatesOfElectionQueryHandler : IRequestHandler<GetCandidatesOfElectionQuery, List<CandidateModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetCandidatesOfElectionQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CandidateModel>> Handle(GetCandidatesOfElectionQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _dbContext.Candidates.Where(x => x.ElectionId == request.ElectionId && x.IsApproved == true).ToListAsync(cancellationToken);

        var result = _mapper.Map<List<CandidateModel>>(candidates);
        return result;

    }
}