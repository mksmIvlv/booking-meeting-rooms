using Application.Interfaces;
using Confluent.Kafka;
using Contracts.Interface;
using Infrastructure.Interfaces.Connections;
using Newtonsoft.Json;

namespace Application.Services.Kafka;

public class KafkaService<T> : IPublishBusService<T> where T : IMessage
{
    #region Поле

    /// <summary>
    /// Подключение к Kafka
    /// </summary>
    private IConnectionKafka _connection;

    #endregion

    #region Конструктор

    public KafkaService(IConnectionKafka connection)
    {
        _connection = connection;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка сообщений в очередь
    /// </summary>
    /// <param name="classMessage">Класс "Сообщение"</param>
    public async Task SendMessageAsync(T classMessage)
    {
        var tempString = JsonConvert.SerializeObject(classMessage);
        
        await _connection.Producer.ProduceAsync(_connection.Settings.TopicName, new Message<Null, string>
        {
            Value = tempString
        });
    }

    #endregion
}