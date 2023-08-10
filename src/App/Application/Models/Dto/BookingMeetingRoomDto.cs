namespace Application.Models.Dto;

/// <summary>
/// Dto брони
/// </summary>
public class BookingMeetingRoomDto
{
    #region Свойства

    /// <summary>
    /// Дата бронирования
    /// </summary>
    public DateOnly DateMeeting { get; set; }
    
    /// <summary>
    /// Время начала бронирования
    /// </summary>
    public TimeOnly StartTimeMeeting { get; set; }
    
    /// <summary>
    /// Время конца бронирования
    /// </summary>
    public TimeOnly EndTimeMeeting { get; set; }
    
    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid MeetingRoomDtoId { get; set; }

    #endregion
}