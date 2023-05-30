using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class CandidateResult : BaseObject
{
    public long CandidateId { get; set; }
    public string TotalVotes { get; set; }
}
