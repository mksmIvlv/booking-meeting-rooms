using Application.Mediatr.Interfaces.Queries;
using Application.Models.Dto;

namespace Application.Mediatr.Features.Models;

/// <summary>
/// Запрос на получение расписания комнаты
/// </summary>
public class GetScheduleSpecificRoomQueries : IQuery<MeetingRoomDto>
{
    #region Свойства

    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid IdRoom { get; }

    #endregion

    #region Конструктор

    public GetScheduleSpecificRoomQueries(Guid idRoom)
    {
        IdRoom = idRoom;
    }

    #endregion
}