namespace Adapter;

public class OldSmsService
{
    public void SendSms(string phoneNumber, string message)
    {
        Console.WriteLine($"Отправка SMS на {phoneNumber}: {message}");
    }
}

public interface INotificationService
{
    void Send(string recipient, string text);
}

public class SmsServiceAdapter : INotificationService
{
    private readonly OldSmsService _oldSmsService;

    public SmsServiceAdapter(OldSmsService oldSmsService)
    {
        _oldSmsService = oldSmsService;
    }

    public void Send(string recipient, string text)
    {
        // Адаптер вызывает старый метод, преобразуя входные данные
        _oldSmsService.SendSms(recipient, text);
    }
}

public class NotificationManager
{
    private readonly INotificationService _notificationService;

    public NotificationManager(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void SendAlert(string user, string message)
    {
        _notificationService.Send(user, message);
    }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        var oldSmsService = new OldSmsService();

        var adaptedSmsService = new SmsServiceAdapter(oldSmsService);

        var manager = new NotificationManager(adaptedSmsService);
        manager.SendAlert("+7-938-535-52-18", "Ваш заказ готов!");
    }
}