using Ballot.Domain.Entities;

namespace Ballot.Application.Common.Models;

public class ElectionModel : BaseModel
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
