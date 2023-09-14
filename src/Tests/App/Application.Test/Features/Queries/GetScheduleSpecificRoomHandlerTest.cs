using System.Reflection;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Features.Queries;
using Application.Mediatr.Interfaces.Queries;
using Application.Models.Dto;
using AutoMapper;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using NSubstitute;
using NUnit.Framework;

namespace Application.Test.Features.Queries;

[TestFixture]
public class GetScheduleSpecificRoomHandlerTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private IQueryHandler<GetScheduleSpecificRoomQuery, MeetingRoomDto> _handler;
    
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
    public void GetScheduleSpecificRoomHandlerTestSetUp()
    {
        _token = new CancellationToken();
        _repositoryMoq = Substitute.For<IRepository>();
        _mapperMoq = Substitute.For<IMapper>();
        _handler = new GetScheduleSpecificRoomHandler(_repositoryMoq, _mapperMoq);
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
        // Модель
        var meetingRoom = new MeetingRoom("Тестовая комната 1", "Описание комнаты");
        // Dto
        var meetingRoomDto = new MeetingRoomDto()
        {
            Id = meetingRoom.Id,
            Name = meetingRoom.Name,
            Description = meetingRoom.Description,
            BookingMeetingRoomsDto = new List<BookingMeetingRoomDto>()
        };
        // Запрос
        var query = new GetScheduleSpecificRoomQuery(meetingRoom.Id);
        // Мок
        _repositoryMoq.GetScheduleAsync(meetingRoom.Id)
            .Returns(meetingRoom);
        _mapperMoq.Map<MeetingRoomDto>(meetingRoom)
            .Returns(meetingRoomDto);
        
        //Act
        var actual = _handler.Handle(query, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.GetType(), Is.EqualTo(meetingRoomDto.GetType()));
        Assert.That(actual.Id, Is.EqualTo(meetingRoom.Id));
        
        _repositoryMoq.Received(1)
            .GetScheduleAsync(Arg.Any<Guid>());
        _mapperMoq.Received(1)
            .Map<MeetingRoomDto>(Arg.Any<MeetingRoom>());
    }
    
    #endregion
}