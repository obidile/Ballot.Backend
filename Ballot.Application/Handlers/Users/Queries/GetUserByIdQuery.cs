using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Users.Queries;

public class GetUserByIdQuery : IRequest<string>
{
    public long UserId { get; set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetUserByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Users.AsQueryable().Where(x => x.Id == request.UserId).ProjectTo<UserModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        if (data == null)
        {
            throw new Exception("No User with the specified ID was found.");
        }

        return data.ToString();
    }
}