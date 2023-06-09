﻿namespace Ballot.Domain.Common;

public class BaseObject
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdateDate { get; set; }
    public string UpdatedBy { get; set; }
}
