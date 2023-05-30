namespace Ballot.Domain.Entities;

public class NotificationSetting
{
    public int Id { get; set; }
    public string Key { get; set; } // e.g. "EmailNotificationsEnabled"
    public string Value { get; set; } // e.g. "true" or "false"
}
