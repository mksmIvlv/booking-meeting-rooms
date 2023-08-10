using Notification.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Notification.Infrastructure.Interfaces.Connections;
using Notification.Infrastructure.Settings.RabbitMQ;
using RabbitMQ.Client;

namespace Notification.Infrastructure.Connections;

/// <summary>
/// Подключение к Rabbit
/// </summary>
public class ConnectionRabbit: IConnectionRabbit
{
    #region Свойства

    /// <summary>
    /// Канал для пользователей
    /// </summary>
    public IModel Channel { get; private set; }

    /// <summary>
    /// Настройки RabbitMq
    /// </summary>
    public RabbitMqSettings Settings { get; }

    #endregion

    #region Конструктор

    public ConnectionRabbit(IOptions<NotificationInfrastructureSettings> settings)
    {
        Settings = settings.Value.RabbitMqSettings;
        ConnectRabbitMQ();
    }

    #endregion

    #region Метод

    /// <summary>
    /// Подключение к RabbitMQ
    /// </summary>
    private void ConnectRabbitMQ()
    {
        // Создание фабрики (ввод логина и пароля)
        var factory = new ConnectionFactory()
        {
            HostName = Settings.ConnectionStringRabbitMQ, 
            VirtualHost = Settings.VirtualHost,
            UserName = Settings.LoginRabbitMQ, 
            Password = Settings.PasswordRabbitMQ
        };

        // Создание подключения
        var connect = factory.CreateConnection();
        
        // Создание модели для пользователей
        Channel = connect.CreateModel();
    }

    #endregion
}