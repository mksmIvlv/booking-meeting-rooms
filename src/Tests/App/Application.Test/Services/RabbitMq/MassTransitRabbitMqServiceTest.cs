using Application.Interfaces;
using Application.Services.RabbitMq;
using Contracts.Interface;
using Contracts.Model;
using Infrastructure.Settings;
using Infrastructure.Settings.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Services.RabbitMq;

[TestFixture]
public class MassTransitRabbitMqServiceTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IPublishBusService<IMessage> _massTransitRabbitMqService;
    
    /// <summary>
    /// Подключение к серверу
    /// </summary>
    private IBus _busMoq;

    #endregion

    #region Методы

    [SetUp]
    public void MassTransitRabbitMqServiceTestSetUp()
    {
        var rabbitMqSettings = Substitute.For<RabbitMqSettings>();
        rabbitMqSettings.NameProvider = "NameProvider1";
        rabbitMqSettings.ConnectionString = "ConnectionString1";
        rabbitMqSettings.Queue = "Queue1";
        var infrastructureSettings = Options.Create(new InfrastructureSettings(){ RabbitMqSettings = rabbitMqSettings });

        _busMoq = Substitute.For<IBus>();
        _massTransitRabbitMqService = new MassTransitRabbitMqService<IMessage>(_busMoq, infrastructureSettings);
    }
    
    [Test, Description("Тест на отправку сообщения в шину.")]
    public void SendMessageAsyncTest()
    {
        // Arrange
        var message = new MessageNotification(123456789, "Тестовое сообщение");
        var uri = new Uri("NameProvider1://ConnectionString1/Queue1");
        var endpoint = Substitute.For<ISendEndpoint>();
        
        _busMoq.GetSendEndpoint(uri).Returns(endpoint);
        
        // Act
        _massTransitRabbitMqService.SendMessageAsync(message)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _busMoq.Received(1).GetSendEndpoint(Arg.Any<Uri>());
        //ToDo Как проверить вызов endpoint.Send()
    }

    #endregion
}