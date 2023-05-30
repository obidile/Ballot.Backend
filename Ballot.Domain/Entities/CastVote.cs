using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class CastVote : BaseObject
{
    public User User { get; set; }
    public Election Election { get; set; }
    public Candidate Candidate { get; set; }
}
