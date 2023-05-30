using Ballot.Application.Common.Interface;
using Ballot.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Users.Command;

public class ChangeUserStatusCommand : IRequest<string>
{
    public long UserId { get; set; }
    public bool IsActive { get; set; }
    public int AdminUserId  { get; set; }
}

public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public ChangeUserStatusCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.AdminUserId, cancellationToken);
        if (adminUser == null)
        {
            return "Admin user does not exist";
        }

        if (adminUser.AccountType != AccountTypeEnum.Admin)
        {
            return "User is not an admin";
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            return "User does not exist";
        }

        var previousStatus = user.IsActive;
        user.IsActive = !user.IsActive;
        user.UpdateDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var newStatus = user.IsActive ? "active" : "inactive";
        return $"User status changed from {previousStatus} to {newStatus}";
    }
}

