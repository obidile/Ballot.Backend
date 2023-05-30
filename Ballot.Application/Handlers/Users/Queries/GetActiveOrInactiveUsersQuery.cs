using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Users.Queries;

public class GetActiveOrInactiveUsersQuery : IRequest<List<UserModel>>
{
    public bool IsActive { get; set; }
}
public class GetActiveOrInactiveUsersQueryHandler : IRequestHandler<GetActiveOrInactiveUsersQuery, List<UserModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetActiveOrInactiveUsersQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> Handle(GetActiveOrInactiveUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsQueryable();

        if (request.IsActive)
        {
            query = query.Where(u => u.IsActive == true);
        }
        else
        {
            query = query.Where(u => !u.IsActive == false);
        }

        var users = await query.ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        return users;
    }
}