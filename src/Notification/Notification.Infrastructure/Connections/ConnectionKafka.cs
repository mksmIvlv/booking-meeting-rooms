using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Notification.Infrastructure.Interfaces.Connections;
using Notification.Infrastructure.Settings;
using Notification.Infrastructure.Settings.Kafka;

namespace Notification.Infrastructure.Connections;

public class ConnectionKafka: IConnectionKafka
{
    #region Поля

    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings Settings { get; }

    /// <summary>
    /// Получатель
    /// </summary>
    public IConsumer<Null, string> Consumer { get; private set; }

    #endregion

    #region Конструктор

    public ConnectionKafka(IOptions<NotificationInfrastructureSettings> settings)
    {
        Settings = settings.Value.KafkaSettings;
        ConnectKafka();
    }

    #endregion

    #region Метод

    /// <summary>
    /// Подключение к серверу Kafka
    /// </summary>
    private void ConnectKafka()
    {
        var config = new ConsumerConfig
        {
            GroupId = Settings.GroupId,
            BootstrapServers = Settings.BootstrapServers,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = Settings.SaslUsername,
            SaslPassword = Settings.SaslPassword
        };
        Consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    #endregion
}