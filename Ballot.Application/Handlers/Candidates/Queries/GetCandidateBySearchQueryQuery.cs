using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Candidates.Queries;

public class GetCandidateBySearchQueryQuery : IRequest<List<CandidateModel>>
{
    public string SearchQuery { get; set; }
}

public class GetCandidateBySearchQueryQueryHandler : IRequestHandler<GetCandidateBySearchQueryQuery, List<CandidateModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public GetCandidateBySearchQueryQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CandidateModel>> Handle(GetCandidateBySearchQueryQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _dbContext.Candidates.Where(x => x.Name.Contains(request.SearchQuery) || x.EmailAddress.Contains(request.SearchQuery))
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<List<CandidateModel>>(candidates);
        return result;
    }
}
