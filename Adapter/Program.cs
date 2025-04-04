using System;
using System.Text.RegularExpressions;

namespace Adapter
{
    public class OldSmsService
    {
        public void SendSms(string phoneNumber, string senderName, string messageText, SmsPriority priority)
        {
            Console.WriteLine($"Отправка SMS через OldSmsService:");
            Console.WriteLine($"Номер: {phoneNumber}");
            Console.WriteLine($"Отправитель: {senderName}");
            Console.WriteLine($"Текст: {messageText}");
            Console.WriteLine($"Приоритет: {priority}");
            Console.WriteLine();
        }
    }

    public enum SmsPriority { High, Medium, Low }
    public interface INotificationService
    {
        void Send(string recipient, string message, bool isUrgent);
    }
    
    public class SmsServiceAdapter : INotificationService
    {
        private readonly OldSmsService _oldSmsService;
        private readonly string _defaultSenderName;

        public SmsServiceAdapter(OldSmsService oldSmsService, string defaultSenderName)
        {
            _oldSmsService = oldSmsService;
            _defaultSenderName = defaultSenderName;
        }

        public void Send(string recipient, string message, bool isUrgent)
        {
            string normalizedPhone = NormalizePhoneNumber(recipient);

            SmsPriority priority = isUrgent ? SmsPriority.High : SmsPriority.Medium;

            _oldSmsService.SendSms(
                phoneNumber: normalizedPhone,
                senderName: _defaultSenderName,
                messageText: message,
                priority: priority
            );
        }
        
        private string NormalizePhoneNumber(string phone)
        {
            // Удаляем все нецифровые символы
            string digits = Regex.Replace(phone, @"[^\d]", "");
            
            if (digits.Length == 11)
            {
                return string.Concat("+7", digits.AsSpan(1));
            }

            return digits;
        }
    }

    public class NotificationManager
    {
        private readonly INotificationService _notificationService;

        public NotificationManager(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void SendAlert(string recipient, string message, bool isUrgent)
        {
            _notificationService.Send(recipient, message, isUrgent);
        }
    }

    internal static class Program
    {
        private static NotificationManager CreateNotificationManager()
        {
            var oldSmsService = new OldSmsService();
            var adaptedSmsService = new SmsServiceAdapter(oldSmsService, "MyApp");
            return new NotificationManager(adaptedSmsService);
        }
        private static void SendSampleNotifications(NotificationManager notificationManager)
        {
            SendUrgentNotification(notificationManager);
            SendRegularNotification(notificationManager);
        }

        private static void SendRegularNotification(NotificationManager manager)
        {
            manager.SendAlert(
                recipient: "+7 999 123-45-67", 
                message: "Напоминание: завтра встреча.", 
                isUrgent: false
            );
        }

        private static void SendUrgentNotification(NotificationManager manager)
        {
            manager.SendAlert(
                recipient: "8 (938) 555-12-34", 
                message: "Срочно! Ваш заказ готов.", 
                isUrgent: true
            );
        }

        private static void Main()
        {
            NotificationManager notificationManager = CreateNotificationManager();
            SendSampleNotifications(notificationManager);
        }
    }
}