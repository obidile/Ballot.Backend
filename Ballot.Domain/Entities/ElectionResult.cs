using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class ElectionResult : BaseObject
{
    public int ElectionId { get; set; }
    public List<CandidateResult> CandidateResults { get; set; }
}
