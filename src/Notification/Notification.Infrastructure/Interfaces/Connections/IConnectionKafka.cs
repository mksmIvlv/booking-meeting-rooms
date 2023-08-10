using Confluent.Kafka;
using Notification.Infrastructure.Settings.Kafka;

namespace Notification.Infrastructure.Interfaces.Connections;

public interface IConnectionKafka
{
    #region Свойства

    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings Settings { get; }

    /// <summary>
    /// Потребитель
    /// </summary>
    public IConsumer<Null, string> Consumer { get; }

    #endregion
}