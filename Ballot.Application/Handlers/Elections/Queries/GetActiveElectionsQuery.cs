using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Elections.Queries;

public class GetActiveElectionsQuery : IRequest<string>
{
}
public class GetActiveElectionsQueryHandler : IRequestHandler<GetActiveElectionsQuery, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetActiveElectionsQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(GetActiveElectionsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Elections.AsNoTracking().Where(x => x.IsActive == true);


        var list = await query.ToListAsync(cancellationToken);

        var result = _mapper.Map<List<ElectionModel>>(list);
        return result.ToString();

    }
}