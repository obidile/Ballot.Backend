using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;

namespace Ballot.Application.Handlers.Candidates.Command;

public class RegisterCandidateCommand : IRequest<string>
{
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public int ElectionId { get; set; }
}
public class RegisterCandidateCommandHandler : IRequestHandler<RegisterCandidateCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IInAppNotificationService _notificationService;
    public RegisterCandidateCommandHandler(IApplicationContext dbContext, IInAppNotificationService notificationService)
    {
        _dbContext = dbContext;
        _notificationService = notificationService;
    }

    public async Task<string> Handle(RegisterCandidateCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.FindAsync(request.ElectionId);

        if (election == null)
        {
            return "Invalid election ID.";
        }

        var candidate = new Candidate
        {
            Name = request.Name,
            EmailAddress = request.EmailAddress,
            Election = election
        };

        _dbContext.Candidates.Add(candidate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var message = $"{candidate.Name} with email address {candidate.EmailAddress} has registered for the {election.Name} election. " +
        $"Please review and approve if all requirements were met or disqualify if they have broken any of the election rules.";
        await _notificationService.SendNotificationAsync(candidate.Id.ToString(), message);

        return "Candidate registered successfully. Approval pending.";
    }
}