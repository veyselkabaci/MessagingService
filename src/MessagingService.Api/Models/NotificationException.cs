namespace MessagingService.Api.Models;

public class NotificationException : Exception
{
    public string Notification { get; set; }
    public string Subject { get; private set; } = "Uyarı";

    public NotificationException(string notification, string subject = "Uyarı") : base(notification)
    {
        this.Notification = notification;
        Subject = subject;
    }

    public NotificationException(Exception innerException, string notification, params object[] args)
        : base(string.Format(notification, args), innerException)
    {
        Notification = notification;
    }
}