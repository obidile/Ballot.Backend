using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class Candidate : BaseObject
{
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public bool IsApproved { get; set; } = false;
    public bool Disqualified { get; set; } = false;
    public int ElectionId { get; set; }
    public Election Election { get; set; }
}
