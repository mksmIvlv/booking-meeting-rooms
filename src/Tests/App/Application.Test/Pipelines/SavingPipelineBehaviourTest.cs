using System.Reflection;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Pipelines;
using Application.Models.Dto;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Pipelines;

[TestFixture]
public class SavingPipelineBehaviourTest
{
    #region Поля

    /// <summary>
    /// Тестируемый сервис
    /// </summary>
    private IPipelineBehavior<PostBookingMeetingRoomCommand, BookingMeetingRoomDto> _pipelineBehavior;
    
    /// <summary>
    /// Доступ к контексту
    /// </summary>
    private IRepository _repositoryMoq;

    #endregion
    
    #region Методы

    [SetUp]
    public void SavingPipelineBehaviourTestSetUp()
    {
        _repositoryMoq = Substitute.For<IRepository>();
        _pipelineBehavior = new SavingPipelineBehaviour<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>(_repositoryMoq);
    }
    
    [Test, Description("Тест на корректное поведение конструктора.")]
    public void ConstructorTest()
    {
        // Arrange
        var fieldContextMoq = _pipelineBehavior.GetType().GetField("_repository",BindingFlags.Instance | BindingFlags.NonPublic);
        
        // Act
        var actualValueContextMoq = fieldContextMoq?.GetValue(_pipelineBehavior);
        
        // Assert
        Assert.That(actualValueContextMoq, Is.EqualTo(_repositoryMoq));
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
                var bookingMeetingRoom = new BookingMeetingRoomDto()
                {
                    DateMeeting = DateOnly.Parse("10.10.2022"),
                    StartTimeMeeting = TimeOnly.Parse("10:00"),
                    EndTimeMeeting = TimeOnly.Parse("11:00")
                };
                
                return bookingMeetingRoom;
            });
        });
        
        //Act
        var actual = _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.DateMeeting, Is.EqualTo(DateOnly.Parse(request.DateMeeting)));
        Assert.That(actual.StartTimeMeeting, Is.EqualTo(TimeOnly.Parse(request.StartTimeMeeting)));
        
        _repositoryMoq.Received(1).SaveAsync();
    }
    
    [Test, Description("Тест проверки метода с запросом, когда сохранение не выполняется.")]
    public void HandleQueryTest()
    {
        // Arrange
        IPipelineBehavior<GetScheduleSpecificRoomQuery, MeetingRoomDto> _pipelineBehavior = 
            new SavingPipelineBehaviour<GetScheduleSpecificRoomQuery, MeetingRoomDto>(_repositoryMoq);
        var guidRoom = Guid.NewGuid();
        var request = new GetScheduleSpecificRoomQuery(guidRoom);
        var next = new RequestHandlerDelegate<MeetingRoomDto>(() =>
        {
            return Task.Run(() =>
            {
                var meetingRoom = new MeetingRoomDto()
                {
                    Id = guidRoom,
                    Description = "Комната 1"
                };
                
                return meetingRoom;
            });
        });
        
        //Act
        var actual = _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.Id, Is.EqualTo(guidRoom));
        
        _repositoryMoq.DidNotReceive().SaveAsync();
    }

    [Test, Description("Тест проверки метода с запросом, когда сохранение не выполняется и происходит ошибка.")]
    public void HandleQueryExceptionTest()
    {
        // Arrange
        IPipelineBehavior<GetScheduleSpecificRoomQuery, MeetingRoomDto> _pipelineBehavior =
            new SavingPipelineBehaviour<GetScheduleSpecificRoomQuery, MeetingRoomDto>(_repositoryMoq);
        var guidRoom = Guid.NewGuid();
        var request = new GetScheduleSpecificRoomQuery(guidRoom);
        var next = new RequestHandlerDelegate<MeetingRoomDto>(() =>
        {
            return Task.Run(() =>
            {
                var meetingRoom = new MeetingRoomDto()
                {
                    Id = guidRoom,
                    Description = "Комната 1"
                };
                throw new Exception("Вызвана ошибка метода.");

                return meetingRoom;
            });
        });

        //Act
        var exceptionActual = Assert.Throws<Exception>(() => _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult());

        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo("Вызвана ошибка метода."));

        _repositoryMoq.DidNotReceive().SaveAsync();
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
                var bookingMeetingRoom = new BookingMeetingRoomDto()
                {
                    DateMeeting = DateOnly.Parse("10.10.2022"),
                    StartTimeMeeting = TimeOnly.Parse("10:00"),
                    EndTimeMeeting = TimeOnly.Parse("11:00")
                };
                throw new Exception("Вызвана ошибка метода.");
                
                return bookingMeetingRoom;
            });
        });
        
        //Act
        var exceptionActual = Assert.Throws<Exception>(() => _pipelineBehavior.Handle(request, next, new CancellationToken())
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo("Вызвана ошибка метода."));
        
        _repositoryMoq.DidNotReceive().SaveAsync();
    }
    
    #endregion
}