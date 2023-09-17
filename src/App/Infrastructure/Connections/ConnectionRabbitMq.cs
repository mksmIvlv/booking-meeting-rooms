using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings;
using Infrastructure.Settings.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Connections;

/// <summary>
/// Подключения к шине RabbitMq
/// </summary>
public class ConnectionRabbitMq : IConnectionRabbitMq
{
    #region Свойства

    /// <summary>
    /// Канал к которому было создано подключение
    /// </summary>
    public IModel Channel { get; private set; }

    /// <summary>
    /// Настройки RabbitMq
    /// </summary>
    public RabbitMqSettings Settings { get; }

    #endregion

    #region Конструктор

    public ConnectionRabbitMq(IOptions<InfrastructureSettings> settings)
    {
        Settings = settings.Value.RabbitMqSettings;
        ConnectRabbitMQ();
    }

    #endregion

    #region Метод

    /// <summary>
    /// Подключение к RabbitMq
    /// </summary>
    private void ConnectRabbitMQ()
    {
        // Создание фабрики (ввод логина и пароля)
        var factory = new ConnectionFactory()
        {
            HostName = Settings.ConnectionString, 
            VirtualHost = Settings.VirtualHost,
            UserName = Settings.Login, 
            Password = Settings.Password
        };

        // Создание подключения
        var connect = factory.CreateConnection();
        
        // Создание модели
        Channel = connect.CreateModel();
    }

    #endregion
}