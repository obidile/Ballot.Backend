using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Countries.Quries;

public class GetElectionByIdQuery : IRequest<string>
{
    public GetElectionByIdQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }
}
public class GetCountryByIdQueryHandler : IRequestHandler<GetElectionByIdQuery, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetCountryByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(GetElectionByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Elections.AsNoTracking().Where(x => x.Id == request.Id).ProjectTo<ElectionModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        if (data == null)
        {
            return "No Election with the specified ID was found.";
        }

        return data.ToString();
    }
}