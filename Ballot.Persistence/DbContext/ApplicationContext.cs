using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Common.Interfaces;

public class ApplicationContext : DbContext, IApplicationContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
    { }

    public DbSet<Election> Elections { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<Votes> Votes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<CastVote> CastVotes { get; set; }
    public DbSet<CandidateResult> CandidateResults { get; set; }
    public DbSet<NotificationSetting> NotificationSettings { get; set; }
}
