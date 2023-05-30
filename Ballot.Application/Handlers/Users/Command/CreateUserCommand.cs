using Ballot.Application.Common.Helpers;
using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Ballot.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Ballot.Application.Handlers.Users.Command;

public class CreateUserCommand : IRequest<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccountTypeEnum AccountType { get; set; }
    public bool IsActive { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }
}
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IConfiguration _config;
    public CreateUserCommandHandler(IApplicationContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _config = config;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (exist)
        {
            return "User already exist";
        }
        var salt = PasswordHelper.GenerateSalt();
        var hashedPassword = PasswordHelper.HashPassword(request.Password, salt);

        var model = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            AccountType = request.AccountType,
            IsActive = request.IsActive,
            PasswordHash = hashedPassword,
            PasswordSalt = salt,
            CreatedBy = "Self",
            CreatedDate = DateTime.UtcNow,
        };

        var confirm = _config.GetValue<string>("VerificationCode");
        if (request.AccountType == AccountTypeEnum.Admin && request.VerificationCode != confirm)
        {
            return "Verification code is not correct";
        }

        _dbContext.Users.Add(model);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return "User was created successfully";
    }

    
}