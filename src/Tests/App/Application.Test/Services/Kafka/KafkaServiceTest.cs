using Application.Interfaces;
using Application.Services.Kafka;
using Confluent.Kafka;
using Contracts.Interface;
using Contracts.Model;
using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings.Kafka;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Services.Kafka;

[TestFixture]
public class KafkaServiceTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IPublishBusService<IMessage> _kafkaService;
    
    /// <summary>
    /// Подключение к серверу
    /// </summary>
    private IConnectionKafka _connectionMoq;

    #endregion

    #region Методы

    [SetUp]
    public void KafkaServiceTestSetUp()
    {
        var kafkaSettings = Substitute.For<KafkaSettings>();
        kafkaSettings.TopicName = "Topic1";
        kafkaSettings.BootstrapServers = "Server1";
        
        _connectionMoq = Substitute.For<IConnectionKafka>();
        _connectionMoq.Settings.Returns(kafkaSettings);
        
        _kafkaService = new KafkaService<IMessage>(_connectionMoq);
    }
    
    [Test, Description("Тест на отправку сообщения в шину.")]
    public void SendMessageAsyncTest()
    {
        // Arrange
        var message = new MessageNotification(123456789, "Тестовое сообщение");
        
        // Act
        _kafkaService.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _connectionMoq.Received(1).Producer.ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>());
    }

    #endregion
}