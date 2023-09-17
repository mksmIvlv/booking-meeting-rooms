using Confluent.Kafka;
using Infrastructure.Settings.Kafka;

namespace Infrastructure.Interfaces.Connections;

/// <summary>
/// Интерфейс для подключения к шине Kafka
/// </summary>
public interface IConnectionKafka
{
    #region Свойства

    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings Settings { get; }
    
    /// <summary>
    /// Продюсер
    /// </summary>
    public IProducer<Null, string> Producer { get; }

    #endregion
}