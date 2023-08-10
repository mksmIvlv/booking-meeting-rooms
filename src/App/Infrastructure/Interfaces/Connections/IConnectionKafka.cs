using Confluent.Kafka;
using Infrastructure.Settings.Kafka;

namespace Infrastructure.Interfaces.Connections;

public interface IConnectionKafka
{
    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings Settings { get; }
    
    /// <summary>
    /// Продюсер
    /// </summary>
    public IProducer<Null, string> Producer { get; }
}