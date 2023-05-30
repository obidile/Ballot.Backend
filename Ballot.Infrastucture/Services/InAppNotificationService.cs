using Ballot.Application.Common.Interface;
using Ballot.Application.Common.Models;
using Ballot.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Ballot.Infrastucture.Services;
public class InAppNotificationService : IInAppNotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IApplicationContext _dbContext;

    public InAppNotificationService(IHubContext<NotificationHub> hubContext, IApplicationContext dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }
    public async Task SendNotificationAsync(string userId, string message)
    {
        var adminUsers = await _dbContext.Users.Where(u => u.AccountType == AccountTypeEnum.Admin).ToListAsync();

        if (!adminUsers.Any())
        {
            return;
        }

        var randomAdminUser = adminUsers.OrderBy(x => Guid.NewGuid()).First();
        var notificationMessage = $"Message: {message}";
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notificationMessage);
    }
}

