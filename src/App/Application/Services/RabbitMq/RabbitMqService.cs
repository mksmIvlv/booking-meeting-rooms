using System.Text;
using Application.Interfaces;
using Contracts.Interface;
using Infrastructure.Interfaces.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Application.Services.RabbitMq;

/// <summary>
/// Сервис для отправки сообщений в шину с помощью RabbitMq
/// </summary>
public class RabbitMqService<T>: IPublishBusService<T> where T: IMessage
{
    #region Поле
    
    /// <summary>
    /// Подключение к RabbitMq
    /// </summary>
    private readonly IConnectionRabbitMq _connectRabbit;

    #endregion

    #region Конструктор

    public RabbitMqService(IConnectionRabbitMq connectRabbit)
    {
        _connectRabbit = connectRabbit;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Отправка сообщений в очередь
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        await Task.Run(() =>
        {
            var tempString = JsonConvert.SerializeObject(classMessage);

            _connectRabbit.Channel.QueueDeclare
            (
                queue: _connectRabbit.Settings.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Создание массива байтов из строки
            var body =  Encoding.UTF8.GetBytes(tempString);

            // Отправка сообщения
            _connectRabbit.Channel.BasicPublish
            (
                exchange: "",
                routingKey: _connectRabbit.Settings.Queue,
                mandatory: false,
                basicProperties: null,
                body: body
            );
        });
    }

    #endregion
}