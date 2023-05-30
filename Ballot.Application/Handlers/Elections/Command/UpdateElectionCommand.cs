using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace Ballot.Application.Handlers.Elections.Command;

public class UpdateElectionCommand : IRequest<string>
{
    public long ElectionId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
public class UpdateElectionCommandHandler : IRequestHandler<UpdateElectionCommand, string>
{
    private readonly IApplicationContext _context;
    public UpdateElectionCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateElectionCommand request, CancellationToken cancellationToken)
    {
        var election = await _context.Elections.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ElectionId, cancellationToken);
        if (election == null)
        {
            return "Election name not found";
        }

        election.Name = request.Name;
        election.StartDate = request.StartDate;
        election.EndDate = request.EndDate;
        election.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return $"{request.Name.ToUpper()} election successfully Updated ";
    }
}