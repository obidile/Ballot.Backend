using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class Votes : BaseObject
{
    public long ElectionId { get; set; }
    public long CandidateId { get; set; }
    public int TotalVotes { get; set; }
    public Election Election { get; set; }
    public Candidate Candidate { get; set; }
}

