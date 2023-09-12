using Application.Mediatr.Features.Models;
using Application.Mediatr.Pipelines;
using Application.Models.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Pipelines;

[TestFixture]
public class LoggingPipelineBehaviourTest
{
    #region Поле

    /// <summary>
    /// Тестируемый сервис
    /// </summary>
    private IPipelineBehavior<PostBookingMeetingRoomCommand, BookingMeetingRoomDto> _pipelineBehavior;
    
    /// <summary>
    /// Логгер
    /// </summary>
    private ILogger<LoggingPipelineBehaviour<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>> _loggerMoq;

    #endregion

    #region Методы

    [SetUp]
    public void LoggingPipelineBehaviourTestSetUp()
    {
        _loggerMoq = Substitute.For<ILogger<LoggingPipelineBehaviour<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>>>();
        _pipelineBehavior = new LoggingPipelineBehaviour<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>(_loggerMoq);
    }

    [Test, Description("Тест проверки успешного выполнения метода.")]
    public void HandleTest()
    {
        // Arrange
        var request = new PostBookingMeetingRoomCommand
            (
                idRoom: Guid.NewGuid(),
                dateMeeting: "10.10.2022",
                startTimeMeeting: "10:00",
                endTimeMeeting: "11:00"
            );
        var next = new RequestHandlerDelegate<BookingMeetingRoomDto>(() =>
        {
            return Task.Run(() =>
            {
                var bookingmeetingRoom = new BookingMeetingRoomDto()
                {
                    DateMeeting = DateOnly.Parse("10.10.2022"),
                    StartTimeMeeting = TimeOnly.Parse("10:00"),
                    EndTimeMeeting = TimeOnly.Parse("11:00")
                };
                
                return bookingmeetingRoom;
            });
        });
        
        //Act
        var actual = _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.DateMeeting, Is.EqualTo(DateOnly.Parse(request.DateMeeting)));
        Assert.That(actual.StartTimeMeeting, Is.EqualTo(TimeOnly.Parse(request.StartTimeMeeting)));
        
        _loggerMoq.Received(1).Log(LogLevel.Information,$"Команда {typeof(PostBookingMeetingRoomCommand)} начала выполнение");
        _loggerMoq.Received(1).Log(LogLevel.Information, $"Команда {typeof(PostBookingMeetingRoomCommand)} закончило выполнение");
        _loggerMoq.DidNotReceive().Log(LogLevel.Warning, $"Команда {typeof(PostBookingMeetingRoomCommand)} закончило выполнение");
    }
    
    [Test, Description("Тест проверки метода с ошибкой.")]
    public void HandleExceptionTest()
    {
        // Arrange
        var request = new PostBookingMeetingRoomCommand
        (
            idRoom: Guid.NewGuid(),
            dateMeeting: "10.10.2022",
            startTimeMeeting: "10:00",
            endTimeMeeting: "11:00"
        );
        var next = new RequestHandlerDelegate<BookingMeetingRoomDto>(() =>
        {
            return Task.Run(() =>
            {
                var bookingmeetingRoom = new BookingMeetingRoomDto()
                {
                    DateMeeting = DateOnly.Parse("10.10.2022"),
                    StartTimeMeeting = TimeOnly.Parse("10:00"),
                    EndTimeMeeting = TimeOnly.Parse("11:00")
                };
                throw new Exception("Вызвана ошибка метода.");
                
                return bookingmeetingRoom;
            });
        });
        
        //Act
        var exceptionActual = Assert.Throws<Exception>(() => _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo("Вызвана ошибка метода."));
        
        _loggerMoq.Received(1).Log(LogLevel.Information,$"Команда {typeof(PostBookingMeetingRoomCommand)} начала выполнение");
        _loggerMoq.Received(1).Log(LogLevel.Warning, $"Команда {typeof(PostBookingMeetingRoomCommand)} закончило выполнение");
    }
    
    #endregion
}