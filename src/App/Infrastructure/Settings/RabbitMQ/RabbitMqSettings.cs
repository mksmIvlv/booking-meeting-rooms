namespace Infrastructure.Settings.RabbitMQ;

/// <summary>
/// Настройки RabbitMq
/// </summary>
public class RabbitMqSettings
{
    #region Свойства

    /// <summary>
    /// Подключение к RabbitMq
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// Подключение к RabbitMq с помощью amqp
    /// </summary>
    public string ConnectionAmqp { get; set; }
    
    /// <summary>
    /// Наименование провайдера
    /// </summary>
    public string NameProvider { get; set; }
    
    /// <summary>
    /// VirtualHost RabbitMq
    /// </summary>
    public string VirtualHost { get; set; }
    
    /// <summary>
    /// Пароль RabbitMq
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Логин RabbitMq
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Название очереди RabbitMq
    /// </summary>
    public string Queue { get; set; }

    #endregion
}