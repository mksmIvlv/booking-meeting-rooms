using System.Reflection;
using Application.Interfaces;
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
public class ReminderNotificationHandlerTest
{
    #region Поля
    
    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private ICommandHandler<GetReminderNotificationCommand, Unit> _handler;

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
    public void PostReminderNotificationHandlerTestSetUp()
    {
        _token = new CancellationToken();
        _repositoryMoq = Substitute.For<IRepository>();
        _publishBusServiceMoq = Substitute.For<IPublishBusService<IMessage>>();
        _handler = new ReminderNotificationHandler(_publishBusServiceMoq, _repositoryMoq);
    }
    
    [Test, Description("Тест на корректное поведение конструктора.")]
    public void ConstructorTest()
    {
        // Arrange
        var fieldRepositoryMoq = _handler.GetType().GetField("_repository",BindingFlags.Instance | BindingFlags.NonPublic);
        var fieldBusServiceMoq = _handler.GetType().GetField("_publishBusService",BindingFlags.Instance | BindingFlags.NonPublic);
        
        // Act
        var actualValueRepositoryMoq = fieldRepositoryMoq?.GetValue(_handler);
        var actualValueBusServiceMoq = fieldBusServiceMoq?.GetValue(_handler);
        
        // Assert
        Assert.That(actualValueRepositoryMoq, Is.EqualTo(_repositoryMoq));
        Assert.That(actualValueBusServiceMoq, Is.EqualTo(_publishBusServiceMoq));
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
        _repositoryMoq.GetRoomsForNotification(dateMeeting, Arg.Any<TimeOnly>(), Arg.Any<TimeOnly>())
            .Returns(collectionBookingMeetingRoom);
        // Команда
        var command = new GetReminderNotificationCommand();
        
        //Act
        _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        _repositoryMoq.Received(1)
            .GetRoomsForNotification(Arg.Any<DateOnly>(), Arg.Any<TimeOnly>(), Arg.Any<TimeOnly>());
        _publishBusServiceMoq.Received(5)
            .SendMessageAsync(Arg.Any<MessageNotification>());
    }
    
    #endregion
}