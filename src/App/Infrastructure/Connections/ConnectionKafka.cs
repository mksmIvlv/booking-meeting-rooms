using Confluent.Kafka;
using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings;
using Infrastructure.Settings.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Connections;

/// <summary>
/// Подключение к Kafka
/// </summary>
public class ConnectionKafka: IConnectionKafka
{
    #region Поля

    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings Settings { get; }

    /// <summary>
    /// Продюсер
    /// </summary>
    public IProducer<Null, string> Producer { get; private set; }

    #endregion

    #region Конструктор

    public ConnectionKafka(IOptions<InfrastructureSettings> settings)
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
        var config = new ProducerConfig
        {
            BootstrapServers = Settings.BootstrapServers,
            //SecurityProtocol = SecurityProtocol.SaslSsl,
            //SaslMechanism = SaslMechanism.Plain,
            //SaslUsername = Settings.SaslUsername,
            //SaslPassword = Settings.SaslPassword
        };
        Producer = new ProducerBuilder<Null, string>(config).Build();
    }

    #endregion
}