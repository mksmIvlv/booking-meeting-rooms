namespace Notification.Infrastructure.Settings.Kafka;

/// <summary>
/// Настройки Kafka
/// </summary>
public class KafkaSettings
{
    #region Свойства

    /// <summary>
    /// Название GroupId
    /// </summary>
    public string GroupId { get; set; }
    
    /// <summary>
    /// Сервер Kafka
    /// </summary>
    public string BootstrapServers { get; set; }
    
    /// <summary>
    /// Название Topic
    /// </summary>
    public string TopicName { get; set; }
    
    /// <summary>
    /// Логин
    /// </summary>
    public string SaslUsername { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string SaslPassword { get; set; }

    #endregion
}