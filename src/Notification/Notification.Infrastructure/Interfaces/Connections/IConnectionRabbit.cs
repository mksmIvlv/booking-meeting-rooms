using Notification.Infrastructure.Settings.RabbitMQ;
using RabbitMQ.Client;

namespace Notification.Infrastructure.Interfaces.Connections;

/// <summary>
/// Подключение к RabbitMq
/// </summary>
public interface IConnectionRabbit
{
    #region Свойства

    /// <summary>
    /// Канал к которому подключен Rabbit, для пользователей
    /// </summary>
    public IModel Channel { get; }

    /// <summary>
    /// Настройки Rabbit
    /// </summary>
    public RabbitMqSettings Settings { get; }

    #endregion
}