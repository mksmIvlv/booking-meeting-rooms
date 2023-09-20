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
public class AddNewItemHandlerTest
{
    #region Поля

    /// <summary>
    /// Доступ к тестируемому сервису
    /// </summary>
    private ICommandHandler<PostAddNewItemCommand, MeetingRoomDto> _handler;
    
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
        _handler = new AddNewItemHandler(_repositoryMoq, _mapperMoq);
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
        var meetingRoom = new MeetingRoom("Комната", "Описание");
        var meetingRoomDto = new MeetingRoomDto()
        {
            Id = meetingRoom.Id,
            Name = meetingRoom.Name,
            Description = meetingRoom.Description,
            BookingMeetingRoomsDto = new List<BookingMeetingRoomDto>(),
            ItemInMeetingRoomsDto = new List<ItemInMeetingRoomDto>()
        };
        // Команда
        var command = new PostAddNewItemCommand(meetingRoom.Id, "Название предмета", "Описание предмета", 1500);
        // Моки
        _repositoryMoq.AddNewItemAsync(meetingRoom.Id, Arg.Any<Item>(), 1500)
            .Returns(meetingRoom);
        _mapperMoq.Map<MeetingRoomDto>(meetingRoom)
            .Returns(meetingRoomDto);
        
        //Act
        var actual = _handler.Handle(command, _token)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.GetType(), Is.EqualTo(meetingRoomDto.GetType()));
        Assert.That(actual.Id, Is.EqualTo(meetingRoom.Id));
        
        _repositoryMoq.Received(1).AddNewItemAsync
        (
            idRoom: Arg.Any<Guid>(),
            item: Arg.Any<Item>(),
            itemPrice: Arg.Any<decimal>()
        );
        _mapperMoq.Received(1).Map<MeetingRoomDto>(Arg.Any<MeetingRoom>());
    }
    
    #endregion
}