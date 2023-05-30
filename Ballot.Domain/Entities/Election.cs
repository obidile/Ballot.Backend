using Ballot.Domain.Common;

namespace Ballot.Domain.Entities;

public class Election : BaseObject
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Candidate> Candidates { get; set; }
    //public ICollection<Voter> Voters { get; set; }
    public ICollection<Vote> Votes { get; set; }
    public ICollection<ElectionResult> Results { get; set; }
}
