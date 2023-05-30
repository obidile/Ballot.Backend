using Ballot.Application.Common.Interface;
using Ballot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace Ballot.Application.Handlers.Elections.Command;

public class CreateElectionCommand : IRequest<string>
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class CreateElectionCommandHandler : IRequestHandler<CreateElectionCommand, string>
{
    private readonly IApplicationContext _context;
    public CreateElectionCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateElectionCommand request, CancellationToken cancellationToken)
    {
        var exist = await _context.Elections.AsNoTracking().AnyAsync(x => x.Name.ToLower() == request.Name.ToLower(), cancellationToken);
        if (exist)
        {
            return "Election name already exist";
        }

        var model = new Election()
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = false,
            CreatedDate = DateTime.UtcNow
        };

        _context.Elections.Add(model);
        await _context.SaveChangesAsync(cancellationToken);

        return $"{request.Name.ToUpper()} election successfully created ";
    }
}