using System.Text;
using Contracts.Models;
using Newtonsoft.Json;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Interfaces.Connections;
using RabbitMQ.Client.Events;

namespace Notification.Application.Services;

/// <summary>
/// Сервис для получения сообщений из шины
/// </summary>
public class RabbitMqService: IConsumerBus
{
    #region Поля
    
    /// <summary>
    /// Подключение к шине
    /// </summary>
    private readonly IConnectionRabbit _connect;

    /// <summary>
    /// Отправка уведомлений
    /// </summary>
    private readonly INotification _notification;
    
    #endregion

    #region Конструктор

    public RabbitMqService(IConnectionRabbit connect, INotification notification)
    {
        _connect = connect;
        _notification = notification;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Получить новые сообщения из шины
    /// </summary>
    public void Listen()
    {
        // Подключиться к каналу пользователей
        _connect.Channel.QueueDeclare
        (
            queue: _connect.Settings.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        // Создать слушателя
        var consumer = new EventingBasicConsumer(_connect.Channel);

        // Получить новые сообщения
        consumer.Received += (ch, ea) =>
        {
            // Конвертировать сообщения в строку
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            var message = JsonConvert.DeserializeObject<MessageNotification>(content);

            // Отправить сообщение в телеграм
            _notification.SendMessage(message.IdChat, message.Text);

            _connect.Channel.BasicAck(ea.DeliveryTag, false);
        };

        _connect.Channel.BasicConsume
        (
            queue: _connect.Settings.Queue,
            autoAck: false,
            consumerTag: "",
            noLocal: false,
            exclusive: false,
            arguments: null,
            consumer: consumer
        );
    }

    #endregion
}