namespace Application.Models.Dto;

/// <summary>
/// Dto комнаты
/// </summary>
public class MeetingRoomDto
{
    #region Свойства

    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название комнаты
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание комнаты
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Расписание комнаты (все брони)
    /// </summary>
    public ICollection<BookingMeetingRoomDto> BookingMeetingRoomsDto { get; set; }
    
    /// <summary>
    /// Коллекция предметов в команте
    /// </summary>
    public ICollection<ItemInMeetingRoomDto> ItemInMeetingRoomsDto { get; set; }
    
    #endregion
}