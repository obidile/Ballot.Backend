using Ballot.Application.Common.Helpers;
using Ballot.Application.Common.Interface;
using Ballot.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Users.Command;

public class UpdateUserCommand : IRequest<string>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccountTypeEnum AccountType { get; set; }
    public bool IsActive { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public UpdateUserCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            return "User does not exist";
        }

        if (user.Email != request.Email && await _dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
        {
            return "Email is already taken";
        }

        if (!string.IsNullOrEmpty(request.OldPassword))
        {
            var isPasswordValid = PasswordHelper.VerifyPassword(request.OldPassword, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid)
            {
                return "Invalid existing password";
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.IsActive = request.IsActive;

        // If new password was provided, update the hashed password and salt
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            var salt = PasswordHelper.GenerateSalt();
            var hashedPassword = PasswordHelper.HashPassword(request.NewPassword, salt);
            user.PasswordSalt = salt;
            user.PasswordHash = hashedPassword;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return "User was updated successfully";
    }
}
