using Application.Interfaces;
using Application.Services.Kafka;
using Contracts.Interface;
using Contracts.Model;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Services.Kafka;

[TestFixture]
public class MassTransitKafkaServiceTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IPublishBusService<IMessage> _massTransitKafkaService;
    
    /// <summary>
    /// Подключение к серверу
    /// </summary>
    private ITopicProducer<IMessage> _topicProducerMoq;

    #endregion

    #region Методы

    [SetUp]
    public void MassTransitKafkaServiceTestSetUp()
    {
        _topicProducerMoq = Substitute.For<ITopicProducer<IMessage>>();
        _massTransitKafkaService = new MassTransitKafkaService<IMessage>(_topicProducerMoq);
    }
    
    [Test, Description("Тест на отправку сообщения в шину.")]
    public void SendMessageAsyncTest()
    {
        // Arrange
        var message = new MessageNotification(123456789, "Тестовое сообщение");
        
        // Act
        _massTransitKafkaService.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _topicProducerMoq.Received(1).Produce(Arg.Any<MessageNotification>());
    }

    #endregion
}