using Application.Interfaces;
using Contracts.Interface;
using MassTransit;

namespace Application.Services.Kafka;

public class MassTransitKafkaService<T>: IPublishBusService<T> where T: IMessage
{
    #region Поле

    /// <summary>
    /// Продюсер
    /// </summary>
    private ITopicProducer<IMessage> _topicProducer;

    #endregion

    #region Конструктор

    public MassTransitKafkaService(ITopicProducer<IMessage> topicProducer)
    {
        _topicProducer = topicProducer;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка сообщений в очередь
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        await _topicProducer.Produce(classMessage);
    }

    #endregion
}