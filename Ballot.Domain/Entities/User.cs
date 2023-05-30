using Ballot.Domain.Common;
using Ballot.Domain.Enums;

namespace Ballot.Domain.Entities;

public class User : BaseObject
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AccountTypeEnum AccountType { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}
