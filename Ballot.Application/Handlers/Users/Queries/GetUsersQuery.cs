using AutoMapper;
using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using Ballot.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Users.Queries;

public class GetUsersQuery : IRequest<List<UserModel>>
{
    public string SearchQuery { get; set; }
    public bool IncludeAdmins { get; set; }
}
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public GetUsersQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchQuery))
        {
            query = query.Where(x => x.FirstName.Contains(request.SearchQuery) || x.LastName.Contains(request.SearchQuery) || x.Email.Contains(request.SearchQuery));
        }

        if (!request.IncludeAdmins)
        {
            query = query.Where(x => x.AccountType != AccountTypeEnum.Admin);
        }

        var users = await query.ToListAsync(cancellationToken);

        var result = _mapper.Map<List<UserModel>>(users);

        return result;
    }
}