using System.Reflection;
using Application.Mediatr.Features.Commands;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;
using AutoMapper;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Features.Commands;

[TestFixture]
public class PostBookingMeetingRoomHandlerTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private ICommandHandler<PostBookingMeetingRoomCommand, BookingMeetingRoomDto> _handler;
    
    /// <summary>
    /// Доступ к репозиторию
    /// </summary>
    private IRepository _repositoryMoq;

    /// <summary>
    /// Доступ к AutoMapper
    /// </summary>
    private IMapper _mapperMoq;

    /// <summary>
    /// Токен
    /// </summary>
    private CancellationToken _token;

    #endregion
    
    #region Методы
    
    [SetUp]
    public void PostBookingMeetingRoomHandlerTestSetUp()
    {
        _repositoryMoq = Substitute.For<IRepository>();
        _mapperMoq = Substitute.For<IMapper>();
        _token = new CancellationToken();
        _handler = new PostBookingMeetingRoomHandler(_repositoryMoq, _mapperMoq);
    }

    [Test, Description("Тест на корректное поведение конструктора.")]
    public void ConstructorTest()
    {
        // Arrange
        var fieldRepositoryMoq = _handler.GetType().GetField("_repository",BindingFlags.Instance | BindingFlags.NonPublic);
        var fieldMapperMoq = _handler.GetType().GetField("_mapper",BindingFlags.Instance | BindingFlags.NonPublic);
        
        // Act
        var actualValueRepositoryMoq = fieldRepositoryMoq?.GetValue(_handler);
        var actualValueMapperMoq = fieldMapperMoq?.GetValue(_handler);
        
        // Assert
        Assert.That(actualValueRepositoryMoq, Is.EqualTo(_repositoryMoq));
        Assert.That(actualValueMapperMoq, Is.EqualTo(_mapperMoq));
    }

    [Test, Description("Тест проверки успешного выполнения метода.")]
    public void HandleTest()
    {
        // Arrange
        var dateMeeting = new DateOnly(2023, 12,12);
        var startTimeMeeting = new TimeOnly(10,00);
        var endTimeMeeting = new TimeOnly(11,00);
        // Модель бронирования
        var bookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, Guid.NewGuid());
        // Dto
        var bookingMeetingRoomDto = new BookingMeetingRoomDto()
        {
            DateMeeting = bookingMeetingRoom.DateMeeting,
            StartTimeMeeting = bookingMeetingRoom.StartTimeMeeting,
            EndTimeMeeting = bookingMeetingRoom.EndTimeMeeting,
            MeetingRoomDtoId = bookingMeetingRoom.IdMeetingRoom
        };
        // Команда
        var command = new PostBookingMeetingRoomCommand(bookingMeetingRoom.Id, dateMeeting.ToString(), startTimeMeeting.ToString(), endTimeMeeting.ToString());
        // Моки
        _repositoryMoq.BookingMeetingRoomAsync(Arg.Any<Guid>(), Arg.Any<DateOnly>(), Arg.Any<TimeOnly>(), Arg.Any<TimeOnly>())
            .Returns(bookingMeetingRoom);
        _mapperMoq.Map<BookingMeetingRoomDto>(bookingMeetingRoom)
            .Returns(bookingMeetingRoomDto);
        
        //Act
        var actual = _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.GetType(), Is.EqualTo(bookingMeetingRoomDto.GetType()));
        Assert.That(actual.DateMeeting, Is.EqualTo(bookingMeetingRoom.DateMeeting));
        
        _repositoryMoq.Received(1).BookingMeetingRoomAsync
            (
                id: Arg.Any<Guid>(),
                dateMeeting: Arg.Any<DateOnly>(),
                startTimeMeeting: Arg.Any<TimeOnly>(),
                endTimeMeeting: Arg.Any<TimeOnly>()
            );
        _mapperMoq.Received(1).Map<BookingMeetingRoomDto>(Arg.Any<BookingMeetingRoom>());
    }
    
    [Test, Description("Тест проверки ошибки при проверки даты бронирования.")]
    public void HandleExceptionDataTest()
    {
        // Arrange
        var dateMeeting = "Не верная дата";
        var startTimeMeeting = new TimeOnly(10,00);
        var endTimeMeeting = new TimeOnly(11,00);
        // Команда
        var command = new PostBookingMeetingRoomCommand(Guid.NewGuid(), dateMeeting, startTimeMeeting.ToString(), endTimeMeeting.ToString());
        
        //Act
        var exceptionActual = Assert.Throws<FormatException>(() => _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo($"String '{dateMeeting}' was not recognized as a valid DateOnly."));
        
        _repositoryMoq.DidNotReceive().BookingMeetingRoomAsync
        (
            id: Arg.Any<Guid>(),
            dateMeeting: Arg.Any<DateOnly>(),
            startTimeMeeting: Arg.Any<TimeOnly>(),
            endTimeMeeting: Arg.Any<TimeOnly>()
        );
        _mapperMoq.DidNotReceive().Map<BookingMeetingRoomDto>(Arg.Any<BookingMeetingRoom>());
    }
    
    [Test, Description("Тест проверки ошибки при проверки времени начала бронирования.")]
    public void HandleExceptionStartTimeTest()
    {
        // Arrange
        var dateMeeting = new DateOnly(2023, 12,12);
        var startTimeMeeting = "Не верное время бронирования";
        var endTimeMeeting = new TimeOnly(11,00);
        // Команда
        var command = new PostBookingMeetingRoomCommand(Guid.NewGuid(), dateMeeting.ToString(), startTimeMeeting, endTimeMeeting.ToString());
        
        //Act
        var exceptionActual = Assert.Throws<FormatException>(() => _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo($"String '{startTimeMeeting}' was not recognized as a valid TimeOnly."));
        
        _repositoryMoq.DidNotReceive().BookingMeetingRoomAsync
        (
            id: Arg.Any<Guid>(),
            dateMeeting: Arg.Any<DateOnly>(),
            startTimeMeeting: Arg.Any<TimeOnly>(),
            endTimeMeeting: Arg.Any<TimeOnly>()
        );
        _mapperMoq.DidNotReceive().Map<BookingMeetingRoomDto>(Arg.Any<BookingMeetingRoom>());
    }
    
    [Test, Description("Тест проверки ошибки при проверки времени конца бронирования.")]
    public void HandleExceptionEndTimeTest()
    {
        // Arrange
        var dateMeeting = new DateOnly(2023, 12,12);
        var startTimeMeeting = new TimeOnly(10,00);
        var endTimeMeeting = "Не верное время бронирования";
        // Команда
        var command = new PostBookingMeetingRoomCommand(Guid.NewGuid(), dateMeeting.ToString(), startTimeMeeting.ToString(), endTimeMeeting);
        
        //Act
        var exceptionActual = Assert.Throws<FormatException>(() => _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo($"String '{endTimeMeeting}' was not recognized as a valid TimeOnly."));
        
        _repositoryMoq.DidNotReceive().BookingMeetingRoomAsync
        (
            id: Arg.Any<Guid>(),
            dateMeeting: Arg.Any<DateOnly>(),
            startTimeMeeting: Arg.Any<TimeOnly>(),
            endTimeMeeting: Arg.Any<TimeOnly>()
        );
        _mapperMoq.DidNotReceive().Map<BookingMeetingRoomDto>(Arg.Any<BookingMeetingRoom>());
    }

    #endregion
}