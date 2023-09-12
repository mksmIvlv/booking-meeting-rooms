using Application.Interfaces;
using Application.Services.RabbitMq;
using Contracts.Interface;
using Contracts.Model;
using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings.RabbitMQ;
using NSubstitute;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Application.Test.Services.RabbitMq;

[TestFixture]
public class RabbitMqServiceTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IPublishBusService<IMessage> _rabbitMqService;
    
    /// <summary>
    /// Подключение к серверу
    /// </summary>
    private IConnectionRabbitMq _connectionMoq;

    #endregion

    #region Методы

    [SetUp]
    public void RabbitMqServiceTestSetUp()
    {
        var rabbitMqSettings = Substitute.For<RabbitMqSettings>();
        rabbitMqSettings.ConnectionString = "ConnectionString1";
        rabbitMqSettings.VirtualHost = "VirtualHost1";
        rabbitMqSettings.Login = "Login1";
        rabbitMqSettings.Password = "Password1";
        
        _connectionMoq = Substitute.For<IConnectionRabbitMq>();
        _connectionMoq.Settings.Returns(rabbitMqSettings);
        
        _rabbitMqService = new RabbitMqService<IMessage>(_connectionMoq);
    }
    
    [Test, Description("Тест на отправку сообщения в шину.")]
    public void SendMessageAsyncTest()
    {
        // Arrange
        var message = new MessageNotification(123456789, "Тестовое сообщение");
        
        // Act
        _rabbitMqService.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _connectionMoq.Channel.Received(1).QueueDeclare
            (
                queue: Arg.Any<string>(),
                durable: Arg.Any<bool>(),
                exclusive: Arg.Any<bool>(),
                autoDelete: Arg.Any<bool>(),
                arguments: Arg.Any<IDictionary<string, object>>()
           );
        _connectionMoq.Channel.Received(1).BasicPublish
            (
                exchange: Arg.Any<string>(),
                routingKey: Arg.Any<string>(),
                mandatory: Arg.Any<bool>(),
                basicProperties: Arg.Any<IBasicProperties>(),
                body:  Arg.Any<ReadOnlyMemory<byte>>()
            );
    }

    #endregion
}