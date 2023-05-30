using Ballot.Application.Common.Interface;
using MediatR;

namespace Ballot.Application.Handlers.Users.Command;

public class DeleteUserCommand : IRequest<string>
{
    public long UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public DeleteUserCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(request.UserId);

        if (user == null)
        {
            return "User not found";
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return "User was deleted successfully";
    }
}
