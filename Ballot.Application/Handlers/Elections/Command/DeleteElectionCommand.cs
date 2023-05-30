using Ballot.Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Application.Handlers.Countries.Command;

public class DeleteElectionCommand : IRequest<string>
{
    public long Id { get; set; }
    public string Name { get; set; }
}
public class DeleteCountryCommandHandler : IRequestHandler<DeleteElectionCommand, string>
{
    private readonly IApplicationContext _dbContext;
    public DeleteCountryCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(DeleteElectionCommand request, CancellationToken cancellationToken)
    {
        var election = await _dbContext.Elections.FirstOrDefaultAsync(x => x.Name == request.Name || x.Id == request.Id, cancellationToken);
        if (election == null)
        {
            return "Election not found";
        }

        _dbContext.Elections.Remove(election);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return "Election was deleted Successfully";
    }
}