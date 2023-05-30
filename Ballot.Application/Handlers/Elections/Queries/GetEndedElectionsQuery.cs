using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Elections.Queries;

public class GetEndedElectionsQuery : IRequest<List<ElectionModel>>
{
}
public class GetEndedElectionsQueryHandler : IRequestHandler<GetEndedElectionsQuery, List<ElectionModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetEndedElectionsQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ElectionModel>> Handle(GetEndedElectionsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var endedElections = await _dbContext.Elections.AsNoTracking().Where(x => x.EndDate <= now).ToListAsync(cancellationToken);
        var result = _mapper.Map<List<ElectionModel>>(endedElections);

        return result;
    }
}