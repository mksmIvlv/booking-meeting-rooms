﻿using Application.Interfaces;
using Application.Mediatr.Features.Commands;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Contracts.Interface;
using Contracts.Model;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using MediatR;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Features.Commands;

[TestFixture]
public class PostUnbookingMeetingRoomHandlerTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private ICommandHandler<PostUnbookingMeetingRoomCommand, Unit> _handler;
    
    /// <summary>
    /// Доступ к репозиторию
    /// </summary>
    private IRepository _repositoryMoq;

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
    public void PostUnbookingMeetingRoomHandlerTestSetUp()
    {
        _token = new CancellationToken();
        _repositoryMoq = Substitute.For<IRepository>();
        _publishBusServiceMoq = Substitute.For<IPublishBusService<IMessage>>();
        _handler = new PostUnbookingMeetingRoomHandler(_repositoryMoq, _publishBusServiceMoq);
    }

    [Test, Description("Тест проверки успешного выполнения метода.")]
    public void HandleTest()
    {
        // Arrange
        var dateMeeting = DateOnly.FromDateTime(DateTime.Now);
        var startTimeMeeting = TimeOnly.FromDateTime(DateTime.Now);
        var endTimeMeeting = TimeOnly.FromDateTime(DateTime.Now).AddHours(1);
        // Коллекция моделей бронирования
        var collectionBookingMeetingRoom = new List<BookingMeetingRoom>()
        {
            new BookingMeetingRoom(dateMeeting.AddDays(1), startTimeMeeting, endTimeMeeting, Guid.NewGuid()),
            new BookingMeetingRoom(dateMeeting.AddDays(2), startTimeMeeting, endTimeMeeting, Guid.NewGuid()),
            new BookingMeetingRoom(dateMeeting.AddDays(3), startTimeMeeting, endTimeMeeting, Guid.NewGuid()),
            new BookingMeetingRoom(dateMeeting.AddDays(4), startTimeMeeting, endTimeMeeting, Guid.NewGuid()),
            new BookingMeetingRoom(dateMeeting.AddDays(5), startTimeMeeting, endTimeMeeting, Guid.NewGuid()),
        };
        
        // Мок
        _repositoryMoq.UnbookingMeetingRoomAsync(dateMeeting, Arg.Any<TimeOnly>());
        _repositoryMoq.UnbookingNotificationAsync(dateMeeting, Arg.Any<TimeOnly>())
            .Returns(collectionBookingMeetingRoom);
        // Команда
        var command = new PostUnbookingMeetingRoomCommand();
        
        //Act
        _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _repositoryMoq.Received(1)
            .UnbookingMeetingRoomAsync(Arg.Any<DateOnly>(), Arg.Any<TimeOnly>());
        _repositoryMoq.Received(1)
            .UnbookingNotificationAsync(Arg.Any<DateOnly>(), Arg.Any<TimeOnly>());
        _publishBusServiceMoq.Received(5)
            .SendMessageAsync(Arg.Any<MessageNotification>());
    }
    
    #endregion
}