using Application.Interfaces;
using Domain.Interfaces.Infrastructure;
using Domain.Models;
using Infrastructure.Interfaces.Connections;

namespace Application.Services;

/// <summary>
/// Работа с комнатами, использует блокировку Redis
/// </summary>
public class RoomService: IRoomService
{
    #region Поля

    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IRepository _repository;

    /// <summary>
    /// Подключение к Redis
    /// </summary>
    private readonly IConnectionRedis _connect;

    #endregion

    #region Конструктор

    public RoomService(IRepository repository, IConnectionRedis connect)
    {
        _repository = repository;
        _connect = connect;
    }

    #endregion

    #region Методы
    
    /// <summary>
    /// Бронирование комнаты
    /// </summary>
    /// <param name="idRoom">Id комнаты</param>
    /// <param name="dateMeeting">Дата бронирования</param>
    /// <param name="startTimeMeeting">Время начала бронирования</param>
    /// <param name="endTimeMeeting">Время конца бронирования</param>
    /// <returns>Информацию о бронировании</returns>
    public async Task<BookingMeetingRoom> BookingRoomAsync(Guid idRoom, DateOnly dateMeeting, TimeOnly startTimeMeeting, TimeOnly endTimeMeeting)
    {
        // Максимальное время блокировки при сбое
        var expiry = TimeSpan.FromSeconds(30);
        
        // Время блокировки ресурса
        var wait = TimeSpan.FromSeconds(10);
        
        // Попытки получить доступ
        var retry = TimeSpan.FromSeconds(1);
        
        await using (var redLock = await _connect.RedLockFactory.CreateLockAsync(_connect.Settings.ConnectionString, expiry, wait, retry))
        {
            if (redLock.IsAcquired)
            {
                var bookingMeetingRoom = 
                    await _repository.BookingMeetingRoomAsync
                        (
                            idRoom, 
                            dateMeeting, 
                            startTimeMeeting, 
                            endTimeMeeting
                        );

                return bookingMeetingRoom;
            }
            else
            {
                throw new Exception("Превышено ожидание для бронирования комнаты.");
            }
        }
    }

    /// <summary>
    /// Разбронирование комнат
    /// </summary>
    /// <returns>Коллекция комнат, которую разбронировали</returns>
    public async Task<List<BookingMeetingRoom>> UnbookingRoomAsync()
    {
        // Максимальное время блокировки при сбое
        var expiry = TimeSpan.FromSeconds(30);
        
        // Время блокировки ресурса
        var wait = TimeSpan.FromSeconds(10);
        
        // Попытки получить доступ
        var retry = TimeSpan.FromSeconds(1);
        
        await using (var redLock = await _connect.RedLockFactory.CreateLockAsync(_connect.Settings.ConnectionString, expiry, wait, retry))
        {
            if (redLock.IsAcquired)
            {
                // Получить текущую дату
                var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
                // Получить текущее время
                var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);

                // Разбронировать комнаты
                await _repository.UnbookingMeetingRoomAsync(currentDateOnly, currentTimeOnly);
                
                // Получить данные для оповещения о разбронировании комнат
                return await _repository.UnbookingNotificationAsync(currentDateOnly, currentTimeOnly);
            }
            else
            {
                throw new Exception("Превышено ожидание для бронирования комнаты.");
            }
        }
    }

    #endregion
}