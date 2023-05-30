namespace Ballot.Application.Common.Interface;

public interface IInAppNotificationService
{
    Task SendNotificationAsync(string userId, string message);
}
