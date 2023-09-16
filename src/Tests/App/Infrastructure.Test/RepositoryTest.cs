using System.Reflection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Infrastructure.Test;

[TestFixture]
public class RepositoryTest
{
    #region Поля
    
    /// <summary>
    /// Доступ к БД
    /// </summary>
    private Context _context;

    /// <summary>
    /// Доступ к репозиторию
    /// </summary>
    private Repository _repository;

    #endregion

    #region Методы
    
    [SetUp]
    public void RepositoryTestSetUp()
    {
        var _dbContext = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new Context(_dbContext);
        _repository = new Repository(_context);
        
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void RepositoryTestTearDown()
    {
        _context.Database.EnsureDeleted();
    }

    [Test, Description("Тест на корректное поведение конструктора.")]
    public void ConstructorTest()
    {
        // Arrange
        var field = _repository.GetType().GetField("_context",BindingFlags.Instance | BindingFlags.NonPublic);
        
        // Act
        var actualValue = field?.GetValue(_repository);
        
        // Assert
        Assert.That(actualValue, Is.EqualTo(_context));
    }

    [Test, Description("Тест бронирования комнаты, если такая комната есть.")]
    public void BookingMeetingRoomAsyncTest()
    {
        // Arrange
        // Создание комнаты
        var meetingRoom = new MeetingRoom("Тестовая комната № 1", "Описание тестовой комнаты № 1");
        // Добавление комнаты в временную бд
        _context.AddRange(meetingRoom);
        _context.SaveChanges();
        // Дата бронирования
        var dateMeeting = new DateOnly(2023, 12,12);
        // Время начала бронирования
        var startTimeMeeting = new TimeOnly(10,00);
        // Время конца бронирования
        var endTimeMeeting = new TimeOnly(11,00);
        
        // Act
        var actual = _repository.BookingMeetingRoomAsync(meetingRoom.Id, dateMeeting, startTimeMeeting, endTimeMeeting)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.Id, Is.EqualTo(meetingRoom.BookingMeetingRooms[0].Id));
        Assert.That(actual.DateMeeting, Is.EqualTo(dateMeeting));
        Assert.That(actual.StartTimeMeeting, Is.EqualTo(startTimeMeeting));
        Assert.That(actual.EndTimeMeeting, Is.EqualTo(endTimeMeeting));
    }

    [Test, Description("Тест бронирования комнаты, если комнаты нет.")]
    public void BookingMeetingRoomAsyncExceptionMeetingRoomNotFoundTest()
    {
        // Arrange
        // Дата бронирования
        var dateMeeting = new DateOnly(2023, 12,12);
        // Время начала бронирования
        var startTimeMeeting = new TimeOnly(10,00);
        // Время конца бронирования
        var endTimeMeeting = new TimeOnly(11,00);
        
        // Act
        var exceptionActual = Assert.Throws<Exception>(() => _repository.BookingMeetingRoomAsync
            ( 
                Guid.NewGuid(),
                dateMeeting, 
                startTimeMeeting,
                endTimeMeeting
            )
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo("комнаты с таким Id нет."));
    }
    
    [Test, Description("Тест получения расписания комнаты, если такая комната есть.")]
    public void GetScheduleAsyncTest()
    {
        // Arrange
        // Дата бронирования
        var dateMeeting = new DateOnly(2023, 12,1);
        // Время начала бронирования
        var startTimeMeeting = new TimeOnly(10,00);
        // Время конца бронирования
        var endTimeMeeting = new TimeOnly(11,00);
        // Создание комнаты
        var meetingRoom = new MeetingRoom("Тестовая комната № 2", "Описание тестовой комнаты № 2");
        // Создание бронирования
        var bookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, meetingRoom.Id);
        // Добавление данных в временную бд
        _context.AddRange(meetingRoom, bookingMeetingRoom);
        _context.SaveChanges();
        
        // Act
        var actual = _repository.GetScheduleAsync(meetingRoom.Id)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.Id, Is.EqualTo(meetingRoom.Id));
        Assert.That(actual.BookingMeetingRooms.Count, Is.EqualTo(meetingRoom.BookingMeetingRooms.Count));
        Assert.That(actual.BookingMeetingRooms[0].Id, Is.EqualTo(meetingRoom.BookingMeetingRooms[0].Id));
    }
    
    [Test, Description("Тест получения расписания комнаты, если комнаты нет.")]
    public void GetScheduleAsyncExceptionMeetingRoomNotFoundTest()
    {
        // Act
        var exceptionActual = Assert.Throws<Exception>(() => _repository.GetScheduleAsync(Guid.NewGuid())
            .GetAwaiter()
            .GetResult());
        
        // Assert
        Assert.That(exceptionActual.Message, Is.EqualTo("комнаты с таким Id нет."));
    }
    
    [Test, Description("Тест разбронирования комнат.")]
    public void UnbookingMeetingRoomAsyncTest()
    {
        // Arrange
        // Текущая дата
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        // Дата бронирования
        var dateMeeting = new DateOnly(2014, 12,1);
        // Время начала бронирования
        var startTimeMeeting = new TimeOnly(10,00);
        // Время конца бронирования
        var endTimeMeeting = new TimeOnly(11,00);
        // Создание комнаты
        var meetingRoom = new MeetingRoom("Тестовая комната № 3", "Описание тестовой комнаты № 3");
        // Создание бронирования
        var bookingMeetingRoom = new BookingMeetingRoom
            (
                dateMeeting, 
                startTimeMeeting, 
                endTimeMeeting, 
                meetingRoom.Id
            );
        var bookingMeetingRoom2 = new BookingMeetingRoom
            (
                dateMeeting,
                currentTimeOnly.AddHours(1),
                currentTimeOnly.AddHours(2),
                meetingRoom.Id
            );
        var bookingMeetingRoom3 = new BookingMeetingRoom
            (
                currentDateOnly.AddDays(1),
                currentTimeOnly.AddHours(1),
                currentTimeOnly.AddHours(2),
                meetingRoom.Id
            );
        // Добавление данных в временную бд
        _context.AddRange(meetingRoom, bookingMeetingRoom, bookingMeetingRoom2, bookingMeetingRoom3);
        _context.SaveChanges();
        
        // Act
        _repository.UnbookingMeetingRoomAsync(currentDateOnly, currentTimeOnly).GetAwaiter().GetResult();
        _context.SaveChanges();
        
        // Assert
        Assert.That(meetingRoom.BookingMeetingRooms.Count(), Is.EqualTo(1));
        Assert.That(_context.BookingMeetingRooms.Count(), Is.EqualTo(1));
        Assert.That(meetingRoom.BookingMeetingRooms[0].Id, Is.EqualTo(bookingMeetingRoom3.Id));
    }
    
    [Test, Description("Тест получения бронировани для отправки оповещения о разбронировании.")]
    public void UnbookingNotificationAsyncTest()
    {
        // Arrange
        // Текущая дата
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        // Создание бронирования
        var bookingMeetingRoom = new BookingMeetingRoom
        (
            new DateOnly(2014, 12,1), 
            new TimeOnly(10,00), 
            new TimeOnly(11,00), 
            Guid.NewGuid()
        );
        var bookingMeetingRoom2 = new BookingMeetingRoom
        (
            new DateOnly(2015, 12,1),
            new TimeOnly(10,00),
            new TimeOnly(11,00),
            Guid.NewGuid()
        );
        var bookingMeetingRoom3 = new BookingMeetingRoom
        (
            currentDateOnly,
            new TimeOnly(00,00),
            new TimeOnly(00,10),
            Guid.NewGuid()
        );
        // Добавление данных в временную бд
        _context.AddRange( bookingMeetingRoom, bookingMeetingRoom2, bookingMeetingRoom3);
        _context.SaveChanges();
        
        // Act
        var actual = _repository.UnbookingNotificationAsync(currentDateOnly, currentTimeOnly)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.Count, Is.EqualTo(3));
        Assert.That(actual[0].Id, Is.EqualTo(bookingMeetingRoom.Id));
    }
    
    [Test, Description("Тест получения бронировани для отправки оповещения о разбронировании, когда броней нет и метод возвращает ноль.")]
    public void UnbookingNotificationAsyncBookingMeetingRoomIsZeroTest()
    {
        // Arrange
        // Текущая дата
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);

        // Act
        var actual = _repository.UnbookingNotificationAsync(currentDateOnly, currentTimeOnly)
            .GetAwaiter()
            .GetResult();

        // Assert
        Assert.That(actual.Count, Is.EqualTo(0));
    }
    
    [Test, Description("Тест получения бронирований для оповещения.")]
    public void GetRoomsForNotificationTest()
    {
        // Arrange
        // Текущая дата
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        // Сдвиг по времени, чтоб был диапазон(10:00 - 11:00)
        var maxTimeOnly = currentTimeOnly.AddHours(1);
        // Создание бронирования
        var bookingMeetingRoom = new BookingMeetingRoom
        (
            DateOnly.FromDateTime(DateTime.Now), 
            TimeOnly.FromDateTime(DateTime.Now).AddMinutes(10), 
            TimeOnly.FromDateTime(DateTime.Now).AddMinutes(20), 
            Guid.NewGuid()
        );
        var bookingMeetingRoom2 = new BookingMeetingRoom
        (
            DateOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now).AddMinutes(30),
            TimeOnly.FromDateTime(DateTime.Now).AddMinutes(40),
            Guid.NewGuid()
        );
        var bookingMeetingRoom3 = new BookingMeetingRoom
        (
            DateOnly.FromDateTime(DateTime.Now),
            new TimeOnly(00,00),
            new TimeOnly(00,10),
            Guid.NewGuid()
        );
        // Добавление данных в временную бд
        _context.AddRange( bookingMeetingRoom, bookingMeetingRoom2, bookingMeetingRoom3);
        _context.SaveChanges();
        
        // Act
        var actual = _repository.GetRoomsForNotification(currentDateOnly, currentTimeOnly, maxTimeOnly);
        
        // Assert
        Assert.That(actual.Count, Is.EqualTo(2));
        Assert.That(actual[0].Id, Is.EqualTo(bookingMeetingRoom.Id));
        Assert.That(actual[1].IsNotification, Is.EqualTo(true));
    }
    
    [Test, Description("Тест получения бронирований для оповещения, если бронирований нет и метод возвращает ноль.")]
    public void GetRoomsForNotificationBookingMeetingRoomIsZeroTest()
    {
        // Arrange
        // Текущая дата
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        // Сдвиг по времени, чтоб был диапазон(10:00 - 11:00)
        var maxTimeOnly = currentTimeOnly.AddHours(1);

        // Act
        var actual = _repository.GetRoomsForNotification(currentDateOnly, currentTimeOnly, maxTimeOnly);

        // Assert
        Assert.That(actual.Count, Is.EqualTo(0));
    }

    [Test, Description("Тест сохранения данных в бд.")]
    public void SaveAsyncTest()
    {
        // Arrange
        var meetingRoom = new MeetingRoom("Тестовая комната № 1", "Описание тестовой комнаты № 1");
        // Добавление комнаты в временную бд
        _context.AddRange(meetingRoom);
        
        // Act
        _repository.SaveAsync()
            .GetAwaiter()
            .GetResult();
        var actual = _context.Set<MeetingRoom>()
            .FirstOrDefaultAsync(e => e.Id == meetingRoom.Id)
            .GetAwaiter()
            .GetResult();
        
        // Assert
        Assert.That(actual.Id, Is.EqualTo(meetingRoom.Id));
    }
    
    #endregion
}