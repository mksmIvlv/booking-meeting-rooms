namespace Infrastructure.Settings.Kafka;

/// <summary>
/// Настройки Kafka
/// </summary>
public class KafkaSettings
{
    #region Свойства

    /// <summary>
    /// Сервер Kafka
    /// </summary>
    public string BootstrapServers { get; set; }
    
    /// <summary>
    /// Название Topic
    /// </summary>
    public string TopicName { get; set; }
    
    /// <summary>
    /// Название Topic для HealthCheck
    /// </summary>
    public string TopicHealth { get; set; }
    
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