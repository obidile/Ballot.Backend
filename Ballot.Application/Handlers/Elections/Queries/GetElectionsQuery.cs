using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Elections.Queries;

public class GetElectionsQuery : IRequest<string>
{
    public string Name { get; set; }
}
public class GetElectionsQueryHandler : IRequestHandler<GetElectionsQuery, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetElectionsQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(GetElectionsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Elections.AsNoTracking();
        query = query.Where(x => x.Name.ToLower() == request.Name.ToLower());


        var list = await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);

        var result = _mapper.Map<List<ElectionModel>>(list);
        return result.ToString();
    }
}