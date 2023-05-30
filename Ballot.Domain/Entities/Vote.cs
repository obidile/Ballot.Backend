using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class Vote : BaseObject
{
    public int VoterId { get; set; }
    public int CandidateId { get; set; }
    public DateTime Timestamp { get; set; }
}
