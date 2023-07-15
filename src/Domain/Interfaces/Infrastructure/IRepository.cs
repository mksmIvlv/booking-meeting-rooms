using Domain.Models;

namespace Domain.Interfaces.Infrastructure;

/// <summary>
/// Репозиторий
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Бронирование комнаты
    /// </summary>
    /// <param name="id">Id комнаты</param>
    /// <param name="dateMeeting">Дата брони</param>
    /// <param name="startTimeMeeting">Начала брони</param>
    /// <param name="endTimeMeeting">Конец брони</param>
    /// <returns>Данные о бронировании</returns>
    public Task<BookingMeetingRoom> BookingMeetingRoomAsync(Guid id, DateOnly dateMeeting, TimeOnly startTimeMeeting, TimeOnly endTimeMeeting);

    /// <summary>
    /// Получение расписания комнаты
    /// </summary>
    /// <param name="id">Id комнаты</param>
    /// <returns>Комнату с данными</returns>
    public Task<MeetingRoom> GetScheduleAsync(Guid id);

    /// <summary>
    /// Разбронирование комнат
    /// </summary>
    public Task UnbookingMeetingRoomAsync(DateOnly currentDateOnly, TimeOnly currentTimeOnly);

    /// <summary>
    /// Получение комнат для отправки оповещения о разбронировании
    /// </summary>
    /// <param name="currentDateOnly">Текущая дата</param>
    /// <param name="currentTimeOnly">Текущее время</param>
    /// <returns>Коллекция бронирований</returns>
    public Task<List<BookingMeetingRoom>> UnbookingNotificationAsync(DateOnly currentDateOnly, TimeOnly currentTimeOnly);

    /// <summary>
    /// Получения комнат для отправки оповещения
    /// </summary>
    /// <param name="currentDateOnly">Текущая дата</param>
    /// <param name="currentTimeOnly">Текущее время</param>
    /// <param name="maxTimeOnly">Максимальное время для поиска броней</param>
    /// <returns>Коллекция бронирований</returns>
    public List<BookingMeetingRoom> GetRoomsForNotification(DateOnly currentDateOnly, TimeOnly currentTimeOnly, TimeOnly maxTimeOnly);
}