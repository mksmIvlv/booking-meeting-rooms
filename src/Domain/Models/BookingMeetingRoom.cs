namespace Domain.Models;

/// <summary>
/// Бронирование комнаты
/// </summary>
public class BookingMeetingRoom
{
    #region Свойства

    /// <summary>
    /// Id бронирования
    /// </summary>
    public Guid IdBooking { get; }

    /// <summary>
    /// Дата бронирования
    /// </summary>
    public DateOnly DateMeeting { get; }
    
    /// <summary>
    /// Время начала бронирования
    /// </summary>
    public TimeOnly StartTimeMeeting { get; }
    
    /// <summary>
    /// Время конца бронирования
    /// </summary>
    public TimeOnly EndTimeMeeting { get; }
    
    /// <summary>
    /// Было ли отправленно оповещение о бронировании
    /// </summary>
    public bool IsNotification { get; private set; }
    
    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid MeetingRoomId { get; private set; }

    #endregion

    #region Конструктор
    
    private BookingMeetingRoom() { }

    public BookingMeetingRoom(DateOnly dateMeeting, TimeOnly startTimeMeeting, TimeOnly endTimeMeeting, Guid meetingRoomId)
    {
        IdBooking = Guid.NewGuid();
        DateMeeting = dateMeeting;
        StartTimeMeeting = startTimeMeeting;
        EndTimeMeeting = endTimeMeeting;
        IsNotification = false;
        MeetingRoomId = meetingRoomId;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Установить, что о текущем бронировании было отправленно оповещение
    /// </summary>
    public void SetTrueNotification()
    {
        IsNotification = true;
    }

    #endregion
}