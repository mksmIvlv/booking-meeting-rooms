using Infrastructure.Settings.Grpc;
using Infrastructure.Settings.Kafka;
using Infrastructure.Settings.RabbitMQ;
using Infrastructure.Settings.Redis;
using Microsoft.Extensions.Options;

namespace Infrastructure.Settings;

/// <summary>
/// Настройки инфраструктурного слоя
/// </summary>
public class InfrastructureSettings: IOptions<InfrastructureSettings>
{
    #region Свойства

    public InfrastructureSettings Value => this;
    
    /// <summary>
    /// Строка подключения к бд
    /// </summary>
    public string ConnectionStringDb { get; set; }

    /// <summary>
    /// Настройки Redis
    /// </summary>
    public RedisSettings RedisSettings { get; set; }
    
    /// <summary>
    /// Настройки RabbitMq
    /// </summary>
    public RabbitMqSettings RabbitMqSettings { get; set; }
    
    /// <summary>
    /// Настройки Kafka
    /// </summary>
    public KafkaSettings KafkaSettings { get; set; }
    
    /// <summary>
    /// Настройки Grpc
    /// </summary>
    public GrpcSettings GrpcSettings { get; set; }

    #endregion
}