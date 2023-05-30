using Ballot.Application.Common.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Ballot.Application.Common.Models;

public class NotificationHub : Hub
{
    public async Task SendNotificationAsync(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}
