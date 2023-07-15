using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Доступ к бд
/// </summary>
public class Context : DbContext
{
    #region Свойства

    /// <summary>
    /// Доступ к комнатам
    /// </summary>
    public DbSet<MeetingRoom> MeetingRooms { get; set; }

    /// <summary>
    /// Доступ к бронированию комнат
    /// </summary>
    public DbSet<BookingMeetingRoom> BookingMeetingRooms { get; set; }

    #endregion
    
    #region Конструктор

    /// <summary>
    /// Конструктор для DI
    /// </summary>
    /// <param name="options">Строка подключения</param>
    public Context(DbContextOptions<Context> options) : base(options) { }

    #endregion
    
    #region Методы

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Repository).Assembly);
        //SeedDataBase(modelBuilder);
    }

    /// <summary>
    /// Заполнение бд начальными данными
    /// </summary>
    private void SeedDataBase(ModelBuilder modelBuilder)
    {
        var meetingRoom1 = new MeetingRoom
        (
            "Переговорная комната 1.", 
            "Описание переговорной комнаты."
        );
        
        var bookingMeetingRoom = new BookingMeetingRoom
        (
            new DateOnly(2023, 10, 25), 
            new TimeOnly(10, 00),
            new TimeOnly(11, 00), 
            meetingRoom1.IdRoom
        );

        var meetingRoom2 = new MeetingRoom
            (
                "Переговорная комната 2.",
                "Описание переговорной комнаты."
            );
        var meetingRoom3 = new MeetingRoom
            (
                "Переговорная комната 3.",
                "Описание переговорной комнаты."
            );
        var meetingRoom4 = new MeetingRoom
            (
                "Переговорная комната 4.",
                "Описание переговорной комнаты."
            );
        var meetingRoom5 = new MeetingRoom
            (
                "Переговорная комната 5.",
                "Описание переговорной комнаты."
            );

        modelBuilder.Entity<MeetingRoom>().HasData(meetingRoom1, meetingRoom2, meetingRoom3, meetingRoom4, meetingRoom5);
        modelBuilder.Entity<BookingMeetingRoom>().HasData(bookingMeetingRoom);
    }
    
    #endregion
}