using Infrastructure.Settings.RabbitMQ;
using RabbitMQ.Client;

namespace Infrastructure.Interfaces.Connections;

/// <summary>
/// Интерфейс для подключения к шине RabbitMq
/// </summary>
public interface IConnectionRabbitMq
{
    /// <summary>
    /// Канал к которому было создано подключение
    /// </summary>
    public IModel Channel { get; }

    /// <summary>
    /// Настройки Rabbit
    /// </summary>
    public RabbitMqSettings Settings { get; }
}