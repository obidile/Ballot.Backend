using Ballot.Application.Common.Interface;
using MediatR;

namespace Ballot.Application.Handlers.Candidates.Command;

public class UpdateCandidateCommand : IRequest<string>
{
    public long CandidateId { get; set; }
    public string Name { get; set; }
    public string EmailAddress { get; set; }
}

public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public UpdateCandidateCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates.FindAsync(request.CandidateId);

        if (candidate == null)
        {
            return "Candidate not found.";
        }
        if (candidate.IsApproved == true)
        {
            return "An Candidate can not update their details";
        }

        candidate.Name = request.Name;
        candidate.EmailAddress = request.EmailAddress;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return $"Candidate {candidate.Name} updated successfully.";
    }
}
