﻿using Application.Interfaces;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Features.Queries;
using Application.Mediatr.Interfaces.Queries;
using Contracts.Interface;
using Contracts.Model;
using MediatR;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Features.Queries;

[TestFixture]
public class PostBookingNotificationHandlerTest
{
    #region Поля
    
    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IQueryHandler<PostBookingNotificationQuery, Unit> _handler;

    /// <summary>
    /// Доступ к сервису для отправки оповещений
    /// </summary>
    private IPublishBusService<IMessage> _publishBusServiceMoq;
    
    /// <summary>
    /// Токен
    /// </summary>
    private CancellationToken _token;

    #endregion
    
    #region Методы
    
    [SetUp]
    public void PostBookingNotificationHandlerTestSetUp()
    {
        _publishBusServiceMoq = Substitute.For<IPublishBusService<IMessage>>();
        _token = new CancellationToken();
        _handler = new PostBookingNotificationHandler(_publishBusServiceMoq);
    }

    [Test, Description("Тест проверки успешного выполнения метода.")]
    public void HandleTest()
    {
        // Arrange
        // Запрос
        var query = new PostBookingNotificationQuery();

        //Act
        _handler.Handle(query, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _publishBusServiceMoq.Received(1)
            .SendMessageAsync(Arg.Any<MessageNotification>());
    }
    
    #endregion
}