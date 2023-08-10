namespace Notification.Infrastructure.Settings.RabbitMQ;

/// <summary>
/// Настройки RabbitMq
/// </summary>
public class RabbitMqSettings
{
    #region Свойства

    /// <summary>
    /// Подключение к RabbitMQ
    /// </summary>
    public string ConnectionStringRabbitMQ { get; set; }
    
    /// <summary>
    /// VirtualHost RabbitMq
    /// </summary>
    public string VirtualHost { get; set; }
    
    /// <summary>
    /// Пароль RabbitMQ
    /// </summary>
    public string PasswordRabbitMQ { get; set; }
    
    /// <summary>
    /// Логин RabbitMQ
    /// </summary>
    public string LoginRabbitMQ { get; set; }
    
    /// <summary>
    /// Название очереди RabbitMQ, для пользователей
    /// </summary>
    public string Queue { get; set; }

    #endregion
}