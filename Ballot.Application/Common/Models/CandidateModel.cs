using Ballot.Domain.Entities;
using System.Runtime.Serialization.Formatters;

namespace Ballot.Application.Common.Models;

public class CandidateModel : BaseModel
{
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public IFieldInfo NIN { get; set; }
    public DateTime DOB { get; set; }
    public bool IsApproved { get; set; }
    public bool Disqualified { get; set; }
    public int ElectionId { get; set; }
    public Election Election { get; set; }
}
