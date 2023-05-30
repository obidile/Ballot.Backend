using Ballot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Common.Interface;

public interface IApplicationContext
{
    DbSet<Election> Elections { get; set; }
    DbSet<Candidate> Candidates { get; set; }
    DbSet<Votes> Votes { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<CastVote> CastVotes { get; set; }
    DbSet<CandidateResult> CandidateResults { get; set; }
    DbSet<NotificationSetting> NotificationSettings { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
