using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;

namespace Application.Mediatr.Features.Models;

/// <summary>
/// Команда для бронирования комнаты
/// </summary>
public class PostBookingMeetingRoomCommand : ICommand<BookingMeetingRoomDto>
{
    #region Свойства

    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid IdRoom { get; }
    
    /// <summary>
    /// Дата бронирования
    /// </summary>
    public string DateMeeting { get; }

    /// <summary>
    /// Время начала бронирования
    /// </summary>
    public string StartTimeMeeting { get; }
    
    /// <summary>
    /// Время конца бронирования
    /// </summary>
    public string EndTimeMeeting { get; }

    #endregion

    #region Конструктор

    public PostBookingMeetingRoomCommand(Guid idRoom, string dateMeeting, string startTimeMeeting, string endTimeMeeting)
    {
        IdRoom = idRoom;
        DateMeeting = dateMeeting;
        StartTimeMeeting = startTimeMeeting;
        EndTimeMeeting = endTimeMeeting;
    }

    #endregion
}