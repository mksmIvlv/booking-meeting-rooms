using Microsoft.Extensions.Options;
using Notification.Infrastructure.Settings.Kafka;
using Notification.Infrastructure.Settings.RabbitMQ;
using Notification.Infrastructure.Settings.Telegram;

namespace Notification.Infrastructure.Settings;

public class NotificationInfrastructureSettings: IOptions<NotificationInfrastructureSettings>
{
    #region Свойства

    public NotificationInfrastructureSettings Value => this;
    
    /// <summary>
    /// Настройки RabbitMQ
    /// </summary>
    public RabbitMqSettings RabbitMqSettings { get; set; }
    
    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings KafkaSettings { get; set; }
    
    /// <summary>
    /// Настройки Telegram
    /// </summary>
    public TelegramSettings TelegramSettings { get; set; }

    #endregion
}