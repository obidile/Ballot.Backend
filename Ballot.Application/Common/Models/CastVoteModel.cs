using Ballot.Domain.Entities;

namespace Ballot.Application.Common.Models;

public class CastVoteModel : BaseModel
{
    public User User { get; set; }
    public Election Election { get; set; }
    public Candidate Candidate { get; set; }
}
